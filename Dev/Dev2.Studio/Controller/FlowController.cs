#pragma warning disable
/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/



using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core;
using Dev2.Activities.Designers2.Decision;
using Dev2.Activities.Designers2.Switch;
using Dev2.Common;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Data.SystemTemplates.Models;
using Dev2.Services.Events;
using Dev2.Studio.Core.AppResources.ExtensionMethods;
using Dev2.Studio.Core.Messages;
using Dev2.Utilities;
using Dev2.Webs;
using Dev2.Webs.Callbacks;
using Newtonsoft.Json;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Studio.Views;


namespace Dev2.Studio.Controller

{
    public interface IFlowController
    {
    }

    public class FlowController : 
        IHandle<ConfigureSwitchExpressionMessage>,
        IHandle<ConfigureCaseExpressionMessage>,
        IHandle<EditCaseExpressionMessage>, 
        IFlowController
    {

        #region Fields

        static readonly IPopupController PopupController = CustomContainer.Get<IPopupController>();
        static Dev2DecisionCallbackHandler _callBackHandler = new Dev2DecisionCallbackHandler();

        #endregion Fields

        #region ctor

        public FlowController()
        {
            EventPublishers.Aggregator.Subscribe(this);
        }

        #endregion ctor

        #region Public Methods

        public static string ConfigureDecisionExpression(ConfigureDecisionExpressionMessage args)
        {
            var condition = ConfigureActivity<DsfFlowDecisionActivity>(args.ModelItem, GlobalConstants.ConditionPropertyText, args.IsNew, args.IsPaste);
            if (condition == null)
            {
                return null;
            }

            var expression = condition.Properties[GlobalConstants.ExpressionPropertyText];

            _callBackHandler = StartDecisionWizard(condition);

            if (_callBackHandler != null)
            {
                try
                {
                    var tmp = FlowNodeHelper.CleanModelData(_callBackHandler.ModelData);
                    var dds = JsonConvert.DeserializeObject<Dev2DecisionStack>(tmp);

                    if (dds == null)
                    {
                        return null;
                    }

                    ActivityHelper.SetArmTextDefaults(dds);
                    var expr = ActivityHelper.InjectExpression(dds, expression);
                    ActivityHelper.SetArmText(args.ModelItem, dds);
                    ActivityHelper.SetDisplayName(args.ModelItem, dds); // PBI 9220 - 2013.04.29 - TWR
                    return expr;
                }
                catch
                {
                    //
                }
            }
            return null;
        }

        public static string ConfigureSwitchExpression(ConfigureSwitchExpressionMessage args)
        {
            OldSwitchValue = string.Empty;
            var expression = ConfigureActivity<DsfFlowSwitchActivity>(args.ModelItem, GlobalConstants.SwitchExpressionPropertyText, args.IsNew, args.IsPaste);
            if (expression == null)
            {
                return null;
            }
            var expressionText = expression.Properties[GlobalConstants.SwitchExpressionTextPropertyText];
            var modelProperty = args.ModelItem.Properties[GlobalConstants.DisplayNamePropertyText];
            if (modelProperty?.Value != null)
            {
                _callBackHandler = StartSwitchDropWizard(expression, modelProperty.Value.ToString());
            }

            if (_callBackHandler != null)
            {
                try
                {
                    var modelData = _callBackHandler.ModelData;
                    var resultSwitch = JsonConvert.DeserializeObject<Dev2Switch>(modelData);
                    var expr = ActivityHelper.InjectExpression(resultSwitch, expressionText);
                    ActivityHelper.SetDisplayName(args.ModelItem, resultSwitch); // MUST use args.ModelItem otherwise it won't be visible!
                    return expr;
                }
                catch (Exception ex)
                {
                    PopupController.Show(GlobalConstants.SwitchWizardErrorString,
                                          GlobalConstants.SwitchWizardErrorHeading, MessageBoxButton.OK,
                                          MessageBoxImage.Error, null, false, true, false, false, false, false);
                }
            }
            return null;
        }

        static Dev2DecisionCallbackHandler StartSwitchDropWizard(ModelItem modelItem, string display)
        {
            var dataContext = new SwitchDesignerViewModel(modelItem, display);
            return ShowSwitchDialogWindow(modelItem, dataContext);
        }

        [ExcludeFromCodeCoverage]
        static Dev2DecisionCallbackHandler ShowSwitchDialogWindow(ModelItem modelItem, SwitchDesignerViewModel dataContext)
        {
            var large = new ConfigureSwitch { DataContext = dataContext };
            var window = new ActivityDefaultWindow();
            if (Application.Current != null)
            {
                window.Style = Application.Current.TryFindResource("SwitchMainWindowStyle") as Style;
            }
            var contentPresenter = window.FindChild<ContentPresenter>();
            if (contentPresenter != null)
            {
                contentPresenter.Content = large;
            }
            var parentContentPane = FindDependencyParent.FindParent<DesignerView>(modelItem?.Parent?.View);
            var dataContext1 = parentContentPane?.DataContext;
            if (dataContext1 != null && dataContext1.GetType().Name == "ServiceTestViewModel")
            {
                window.SetEnableDoneButtonState(false);
            }


            var showDialog = window.ShowDialog();
            window.SetEnableDoneButtonState(true);
            if (showDialog.HasValue && showDialog.Value)
            {
                var callBack = new Dev2DecisionCallbackHandler { ModelData = JsonConvert.SerializeObject(dataContext.Switch) };
                return callBack;
            }
            return null;
        }

        public static void ConfigureSwitchCaseExpression(ConfigureCaseExpressionMessage args)
        {
            OldSwitchValue = string.Empty;
            if (args.ExpressionText != null)
            {
                _callBackHandler = ShowSwitchDragDialog(args.ModelItem, args.ExpressionText);
            }

            if (_callBackHandler != null)
            {
                try
                {
                    var ds = JsonConvert.DeserializeObject<Dev2Switch>(_callBackHandler.ModelData);
                    ActivityHelper.SetSwitchKeyProperty(ds, args.ModelItem);
                }
                catch (Exception ex)
                {
                    PopupController.Show(GlobalConstants.SwitchWizardErrorString,
                                          GlobalConstants.SwitchWizardErrorHeading, MessageBoxButton.OK,
                                          MessageBoxImage.Error, null, false, true, false, false, false, false);
                }
            }
        }

        static Dev2DecisionCallbackHandler ShowSwitchDragDialog(ModelItem modelData, string variable = "", bool isNew = true)
        {
            var switchDesignerViewModel = new SwitchDesignerViewModel(modelData, "") { SwitchVariable = variable };
            if (isNew)
            {
                switchDesignerViewModel.SwitchExpression = string.Empty;
            }
            return ShowSwitchArmDialog(switchDesignerViewModel);
        }

        static Dev2DecisionCallbackHandler ShowSwitchArmDialog(SwitchDesignerViewModel dataContext)
        {
            var large = new ConfigureSwitchArm { DataContext = dataContext };
            var window = new ActivityDefaultWindow();
            if (Application.Current != null)
            {
                window.Style = Application.Current.TryFindResource("SwitchCaseWindowStyle") as Style;
            }
            var contentPresenter = window.FindChild<ContentPresenter>();
            if (contentPresenter != null)
            {
                contentPresenter.Content = large;
            }
            window.SetEnableDoneButtonState(true);

            var showDialog = window.ShowDialog();
            if (showDialog.HasValue && showDialog.Value)
            {
                var callBack = new Dev2DecisionCallbackHandler { ModelData = JsonConvert.SerializeObject(dataContext.Switch) };
                return callBack;
            }
            return null;
        }
        
        public static void TryEditSwitchCaseExpression(EditCaseExpressionMessage args)
        {
            OldSwitchValue = string.Empty;
            var switchCaseValue = args.ModelItem.Properties["Case"];
            var switchVal = args.ModelItem.Properties["ParentFlowSwitch"];
            if (switchVal != null)
            {
                var variable = SwitchExpressionValue(switchVal);
                _callBackHandler = ShowSwitchDragDialog(args.ModelItem, variable, false);
            }
            if (_callBackHandler != null)
            {
                try
                {
                    EditSwitchCaseExpression(switchCaseValue, switchVal);
                }
                catch (Exception ex)
                {
                    PopupController.Show(GlobalConstants.SwitchWizardErrorString,
                                          GlobalConstants.SwitchWizardErrorHeading, MessageBoxButton.OK,
                                          MessageBoxImage.Error, null, false, true, false, false, false, false);
                }
            }
        }

        static void EditSwitchCaseExpression(ModelProperty switchCaseValue, ModelProperty switchVal)
        {
            var ds = JsonConvert.DeserializeObject<Dev2Switch>(_callBackHandler.ModelData);
            if (ds != null)
            {
                var validExpression = true;
                if (switchVal?.ComputedValue is System.Activities.Statements.FlowSwitch<string> flowSwitch && flowSwitch.Cases.Any(flowNode => flowNode.Key == ds.SwitchExpression))
                {
                    validExpression = false;
                }


                if (!validExpression)
                {
                    PopupController.Show(Warewolf.Studio.Resources.Languages.Core.SwitchCaseUniqueMessage, Warewolf.Studio.Resources.Languages.Core.SwitchFlowErrorHeader,
                        MessageBoxButton.OK, MessageBoxImage.Error, "", false, true, false, false, false, false);
                }
                else
                {
                    OldSwitchValue = switchCaseValue?.ComputedValue.ToString();
                    if (switchCaseValue?.ComputedValue.ToString() != ds.SwitchExpression)
                    {
                        switchCaseValue?.SetValue(ds.SwitchExpression);
                    }
                }
            }
        }

        public static string OldSwitchValue { get; private set; }

        static string SwitchExpressionValue(ModelProperty activityExpression)
        {
            var tmpModelItem = activityExpression.Value;

            var switchExpressionValue = string.Empty;

            var tmpProperty = tmpModelItem?.Properties["Expression"];

            if (tmpProperty?.Value != null)
            {
                var value = tmpProperty.ComputedValue as DsfFlowSwitchActivity;
                var tmp = value?.ExpressionText;
                if (!string.IsNullOrEmpty(tmp))
                {
                    var start = tmp.IndexOf("(", StringComparison.Ordinal);
                    var end = tmp.IndexOf(",", StringComparison.Ordinal);

                    if (start < end && start >= 0)
                    {
                        start += 2;
                        end -= 1;
                        switchExpressionValue = tmp.Substring(start, end - start);
                    }
                }
            }
            return switchExpressionValue;
        }
        #endregion public methods

        #region Protected Methods

        static Dev2DecisionCallbackHandler StartDecisionWizard(ModelItem mi)
        {
            var dataContext = new DecisionDesignerViewModel(mi);

            return ShowDecisionDialogWindow(mi, dataContext);
        }

        [ExcludeFromCodeCoverage]
        static Dev2DecisionCallbackHandler ShowDecisionDialogWindow(ModelItem mi, DecisionDesignerViewModel dataContext)
        {
            var large = new Large { DataContext = dataContext };
            var window = new ActivityDefaultWindow();
            if (Application.Current != null)
            {
                window.Style = Application.Current.TryFindResource("DecisionWindowStyle") as Style;
            }
            var contentPresenter = window.FindChild<ContentPresenter>();
            if (contentPresenter != null)
            {
                contentPresenter.Content = large;
            }

            window.SetEnableDoneButtonState(true);
            var parentContentPane = FindDependencyParent.FindParent<DesignerView>(mi?.Parent?.View);
            var dataContext1 = parentContentPane?.DataContext;
            if (dataContext1 != null && dataContext1.GetType().Name == "ServiceTestViewModel")
            {
                window.SetEnableDoneButtonState(false);
            }


            var showDialog = window.ShowDialog();
            window.SetEnableDoneButtonState(true);
            if (showDialog.HasValue && showDialog.Value)
            {
                var dev2DecisionCallbackHandler = new Dev2DecisionCallbackHandler();
                dataContext.GetExpressionText();
                dev2DecisionCallbackHandler.ModelData = dataContext.ExpressionText;
                return dev2DecisionCallbackHandler;
            }
            return null;
        }

        #endregion

        #region IHandle

        public void Handle(ConfigureSwitchExpressionMessage message)
        {
            ConfigureSwitchExpression(message);
        }

        public void Handle(ConfigureCaseExpressionMessage message)
        {
            ConfigureSwitchCaseExpression(message);
        }

        public void Handle(EditCaseExpressionMessage message)
        {
            TryEditSwitchCaseExpression(message);
        }

        #endregion IHandle

        #region ConfigureActivity

        static ModelItem ConfigureActivity<T>(ModelItem modelItem, string propertyName, bool isNew,bool isPaste) where T : class, IFlowNodeActivity, new()
        {
            var property = modelItem.Properties[propertyName];
            if (property == null)
            {
                return null;
            }

            ModelItem result;
            var activity = property.ComputedValue as T;
            if (activity == null)
            {
                activity = new T();
                result = property.SetValue(activity);
            }
            else
            {
                result = property.Value;

                var isCopyPaste = isNew && !string.IsNullOrEmpty(activity.ExpressionText);
                if (result == null || isCopyPaste)
                {                    
                    if (activity is IDev2Activity act && isPaste)
                    {
                        act.UniqueID = Guid.NewGuid().ToString();
                    }
                    return null;
                }
            }
            return result;
        }

        #endregion
    }
}
