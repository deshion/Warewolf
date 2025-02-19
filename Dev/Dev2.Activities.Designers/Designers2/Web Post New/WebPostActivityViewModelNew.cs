/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dev2.Activities.Designers2.Core;
using Dev2.Activities.Designers2.Core.Extensions;
using Dev2.Activities.Designers2.Core.Source;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.ToolBase;
using Dev2.Common.Interfaces.WebService;
using Dev2.Common.Interfaces.WebServices;
using Dev2.Communication;
using Dev2.Providers.Errors;
using Dev2.Studio.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Warewolf.Core;
using Warewolf.Data.Options;
using Warewolf.Options;
using Warewolf.UI;

namespace Dev2.Activities.Designers2.Web_Post_New
{
    public class WebPostActivityViewModelNew : CustomToolWithRegionBase, IWebServiceBaseViewModel
    {
        private IOutputsToolRegion _outputsRegion;
        private IWebPostInputArea _inputArea;
        private ISourceToolRegion<IWebServiceSource> _sourceRegion;
        private readonly ServiceInputBuilder _builder;
        private IErrorInfo _worstDesignError;
        private readonly ModelItem _modelItem;
        private OptionsWithNotifier _options;

        private const string DoneText = "Done";
        private const string FixText = "Fix";
        private const string OutputDisplayName = " - Outputs";

        private readonly string _sourceNotFoundMessage = Warewolf.Studio.Resources.Languages.Core.DatabaseServiceSourceNotFound;

        public WebPostActivityViewModelNew(ModelItem modelItem)
            : base(modelItem)
        {
            _modelItem = modelItem;
            var shellViewModel = CustomContainer.Get<IShellViewModel>();
            var server = shellViewModel.ActiveServer;
            var model = CustomContainer.CreateInstance<IWebServiceModel>(server.UpdateRepository, server.QueryProxy, shellViewModel, server);
            Model = model;
            _builder = new ServiceInputBuilder();
            SetupCommonProperties();
            this.RunViewSetup();
            HelpText = Warewolf.Studio.Resources.Languages.HelpText.Tool_WebMethod_Post;
        }

        public WebPostActivityViewModelNew(ModelItem modelItem, IWebServiceModel model)
            : base(modelItem)
        {
            Model = model;
            _modelItem = modelItem;

            _builder = new ServiceInputBuilder();
            SetupCommonProperties();
        }

        Guid UniqueID => GetProperty<Guid>();

        void SetupCommonProperties()
        {
            AddTitleBarMappingToggle();
            InitialiseViewModel(new ManageWebServiceInputViewModel(this, Model));
            NoError = new ErrorInfo
            {
                ErrorType = ErrorType.None,
                Message = "Service Working Normally"
            };
            if (SourceRegion.SelectedSource == null)
            {
                UpdateLastValidationMemoWithSourceNotFoundError();
            }
            UpdateWorstError();
        }

        void UpdateLastValidationMemoWithSourceNotFoundError()
        {
            var memo = new DesignValidationMemo
            {
                InstanceID = UniqueID,
                IsValid = false,
            };
            memo.Errors.Add(new ErrorInfo
            {
                InstanceID = UniqueID,
                ErrorType = ErrorType.Critical,
                FixType = FixType.None,
                Message = _sourceNotFoundMessage
            });
            UpdateDesignValidationErrors(memo.Errors);
        }

        public void ClearValidationMemoWithNoFoundError()
        {
            var memo = new DesignValidationMemo
            {
                InstanceID = UniqueID,
                IsValid = false,
            };
            memo.Errors.Add(new ErrorInfo
            {
                InstanceID = UniqueID,
                ErrorType = ErrorType.None,
                FixType = FixType.None,
                Message = ""
            });
            UpdateDesignValidationErrors(memo.Errors);
        }

        void UpdateDesignValidationErrors(IEnumerable<IErrorInfo> errors)
        {
            DesignValidationErrors.Clear();
            foreach (var error in errors)
            {
                DesignValidationErrors.Add(error);
            }
            UpdateWorstError();
        }

        public override void Validate()
        {
            if (Errors == null)
            {
                Errors = new List<IActionableErrorInfo>();
            }
            Errors.Clear();

            Errors = Regions.SelectMany(a => a.Errors).Select(a => new ActionableErrorInfo(new ErrorInfo() { Message = a, ErrorType = ErrorType.Critical }, () => { }) as IActionableErrorInfo).ToList();
            if (SourceRegion.Errors.Count > 0)
            {
                foreach (var designValidationError in SourceRegion.Errors)
                {
                    DesignValidationErrors.Add(new ErrorInfo { ErrorType = ErrorType.Critical, Message = designValidationError });
                }

            }
            if (Errors.Count <= 0)
            {
                ClearValidationMemoWithNoFoundError();
            }
            UpdateWorstError();
            InitializeProperties();
        }

        void UpdateWorstError()
        {
            if (DesignValidationErrors.Count == 0)
            {
                DesignValidationErrors.Add(NoError);
            }

            IErrorInfo[] worstError = { DesignValidationErrors[0] };

            foreach (var error in DesignValidationErrors.Where(error => error.ErrorType > worstError[0].ErrorType))
            {
                worstError[0] = error;
                if (error.ErrorType == ErrorType.Critical)
                {
                    break;
                }
            }
            SetWorstDesignError(worstError[0]);
        }

        void SetWorstDesignError(IErrorInfo value)
        {
            if (_worstDesignError != value)
            {
                _worstDesignError = value;
                IsWorstErrorReadOnly = value == null || value.ErrorType == ErrorType.None || value.FixType == FixType.None || value.FixType == FixType.Delete;
                WorstError = value?.ErrorType ?? ErrorType.None;
            }
        }

        void InitialiseViewModel(IManageWebInputViewModel manageServiceInputViewModel)
        {
            ManageServiceInputViewModel = manageServiceInputViewModel;

            BuildRegions();

            LabelWidth = 46;
            ButtonDisplayValue = DoneText;

            ShowLarge = true;
            ThumbVisibility = Visibility.Visible;
            ShowExampleWorkflowLink = Visibility.Collapsed;

            DesignValidationErrors = new ObservableCollection<IErrorInfo>();
            FixErrorsCommand = new Runtime.Configuration.ViewModels.Base.DelegateCommand(o =>
            {
                IsWorstErrorReadOnly = true;
            });

            SetDisplayName("");
            OutputsRegion.OutputMappingEnabled = true;
            TestInputCommand = new DelegateCommand(TestProcedure);

            InitializeProperties();

            if (OutputsRegion != null && OutputsRegion.IsEnabled)
            {
                var recordsetItem = OutputsRegion.Outputs.FirstOrDefault(mapping => !string.IsNullOrEmpty(mapping.RecordSetName));
                if (recordsetItem != null)
                {
                    OutputsRegion.IsEnabled = true;
                }
            }

            LoadConditionExpressionOptions();
        }

        public int LabelWidth { get; set; }

        public List<KeyValuePair<string, string>> Properties { get; private set; }
        void InitializeProperties()
        {
            Properties = new List<KeyValuePair<string, string>>();
            AddProperty("Source :", SourceRegion.SelectedSource == null ? "" : SourceRegion.SelectedSource.Name);
            AddProperty("Type :", Type);
            AddProperty("Url :", SourceRegion.SelectedSource == null ? "" : SourceRegion.SelectedSource.HostName);
        }

        public OptionsWithNotifier ConditionExpressionOptions
        {
            get => _conditionExpressionOptions;
            set
            {
                _conditionExpressionOptions = value;
                OnPropertyChanged(nameof(ConditionExpressionOptions));
                _conditionExpressionOptions.OptionChanged += UpdateConditionExpressionOptionsModelItem;
            }
        }

        private void UpdateConditionExpressionOptionsModelItem()
        {
            if (ConditionExpressionOptions?.Options != null)
            {
                var tmp = OptionConvertor.ConvertToListOfT<FormDataConditionExpression>(ConditionExpressionOptions.Options);
                _modelItem.Properties["Conditions"]?.SetValue(tmp);
                AddEmptyConditionExpression();
                foreach (var item in ConditionExpressionOptions.Options)
                {
                    if (item is FormDataOptionConditionExpression conditionExpression)
                    {
                        conditionExpression.DeleteCommand = new Runtime.Configuration.ViewModels.Base.DelegateCommand(a => 
                        { 
                            RemoveConditionExpression(conditionExpression); 
                        });
                    }
                }
            }
        }

        private void RemoveConditionExpression(FormDataOptionConditionExpression conditionExpression)
        {
            var count = ConditionExpressionOptions.Options.Count(o => o is FormDataOptionConditionExpression optionCondition && optionCondition.IsEmptyRow);
            var empty = conditionExpression.IsEmptyRow;
            var allow = !empty || count > 1;

            if (_conditionExpressionOptions.Options.Count > 1 && allow)
            {
                var list = new List<IOption>(_conditionExpressionOptions.Options);
                list.Remove(conditionExpression);
                ConditionExpressionOptions.Options = list;
                OnPropertyChanged(nameof(ConditionExpressionOptions));
            }
        }

        private void AddEmptyConditionExpression()
        {
            var emptyRows = ConditionExpressionOptions.Options.Where(o => o is FormDataOptionConditionExpression optionCondition && optionCondition.IsEmptyRow);

            if (!emptyRows.Any())
            {
                var conditionExpression = new FormDataOptionConditionExpression(){ IsMultiPart = !InputArea.IsUrlEncodedChecked };
                var list = new List<IOption>(_conditionExpressionOptions.Options)
                {
                    conditionExpression
                };
                ConditionExpressionOptions.Options = list;
                OnPropertyChanged(nameof(ConditionExpressionOptions));
            }
        }

        private void LoadConditionExpressionOptions()
        {
            var conditionExpressionList = _modelItem.Properties["Conditions"]?.ComputedValue as IList<FormDataConditionExpression>;
            if (conditionExpressionList is null)
            {
                conditionExpressionList = new List<FormDataConditionExpression>();
            }
            var result = OptionConvertor.ConvertFromListOfT(conditionExpressionList);
            result.ForEach(r => ((FormDataOptionConditionExpression)r).IsMultiPart = !InputArea.IsUrlEncodedChecked);
            ConditionExpressionOptions = new OptionsWithNotifier { Options = result };
            UpdateConditionExpressionOptionsModelItem();
        }

        void AddProperty(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Properties.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        public IManageWebInputViewModel ManageServiceInputViewModel { get; set; }

        public void TestProcedure()
        {
            if (SourceRegion.SelectedSource != null)
            {
                var service = ToModel();
                ManageServiceInputViewModel.InputArea.Inputs = service.Inputs;
                ManageServiceInputViewModel.IsFormDataChecked = service.IsFormDataChecked;
                ManageServiceInputViewModel.IsUrlEncodedChecked = service.IsUrlEncodedChecked;
                ManageServiceInputViewModel.Model = service;
                ManageServiceInputViewModel.LoadConditionExpressionOptions(ConditionExpressionOptions.Options);

                ManageServiceInputViewModel.IsGenerateInputsEmptyRows = service.Inputs.Count < 1;
                ManageServiceInputViewModel.InputCountExpandAllowed = service.Inputs.Count > 2 || service.FormDataParameters.Count > 2;

                GenerateOutputsVisible = true;
                SetDisplayName(OutputDisplayName);
            }
        }

        IErrorInfo NoError { get; set; }

        public bool IsWorstErrorReadOnly
        {
            get => (bool)GetValue(IsWorstErrorReadOnlyProperty);
            private set
            {
                ButtonDisplayValue = value ? DoneText : FixText;
                SetValue(IsWorstErrorReadOnlyProperty, value);
            }
        }
        public static readonly DependencyProperty IsWorstErrorReadOnlyProperty =
            DependencyProperty.Register("IsWorstErrorReadOnly", typeof(bool), typeof(WebPostActivityViewModelNew), new PropertyMetadata(false));

        public ErrorType WorstError
        {
            get => (ErrorType)GetValue(WorstErrorProperty);
            private set => SetValue(WorstErrorProperty, value);
        }
        public static readonly DependencyProperty WorstErrorProperty =
        DependencyProperty.Register("WorstError", typeof(ErrorType), typeof(WebPostActivityViewModelNew), new PropertyMetadata(ErrorType.None));

        bool _generateOutputsVisible;
        private OptionsWithNotifier _conditionExpressionOptions;

        public DelegateCommand TestInputCommand { get; set; }

        string Type => GetProperty<string>();

        void AddTitleBarMappingToggle()
        {
            HasLargeView = true;
        }

        public void SetDisplayName(string displayName)
        {
            var index = DisplayName.IndexOf(" -", StringComparison.Ordinal);

            if (index > 0)
            {
                DisplayName = DisplayName.Remove(index);
            }

            var displayName2 = DisplayName;

            if (!string.IsNullOrEmpty(displayName2) && displayName2.Contains("Dsf"))
            {
                DisplayName = displayName2;
            }
            if (!string.IsNullOrWhiteSpace(displayName))
            {
                DisplayName = displayName2 + displayName;
            }
        }

        public IHeaderRegion GetHeaderRegion() => InputArea;

        public Runtime.Configuration.ViewModels.Base.DelegateCommand FixErrorsCommand { get; set; }

        public ObservableCollection<IErrorInfo> DesignValidationErrors { get; set; }

        public string ButtonDisplayValue { get; set; }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            mainViewModel?.HelpViewModel.UpdateHelpText(helpText);
        }

        public override IList<IToolRegion> BuildRegions()
        {
            IList<IToolRegion> regions = new List<IToolRegion>();
            if (SourceRegion == null)
            {
                SourceRegion = new WebSourceRegion(Model, ModelItem) { SourceChangedAction = () => { OutputsRegion.IsEnabled = false; } };
                regions.Add(SourceRegion);
                InputArea = new WebPostInputRegion(ModelItem, SourceRegion);
                InputArea.ViewModel = this;
                InputArea.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "PostData" && InputArea.Headers.All(value => string.IsNullOrEmpty(value.Name)))
                    {
                        ((ManageWebServiceInputViewModel)ManageServiceInputViewModel).BuidHeaders(InputArea.PostData);
                    }

                };
                regions.Add(InputArea);
                OutputsRegion = new OutputsRegion(ModelItem, true);
                regions.Add(OutputsRegion);
                if (OutputsRegion.Outputs.Count > 0)
                {
                    OutputsRegion.IsEnabled = true;

                }
                ErrorRegion = new ErrorRegion();
                regions.Add(ErrorRegion);
                SourceRegion.Dependants.Add(InputArea);
                SourceRegion.Dependants.Add(OutputsRegion);
            }
            regions.Add(ManageServiceInputViewModel);
            Regions = regions;
            return regions;
        }
        public ErrorRegion ErrorRegion { get; private set; }

        public IOutputsToolRegion OutputsRegion
        {
            get => _outputsRegion;
            set
            {
                _outputsRegion = value;
                OnPropertyChanged();
            }
        }
        public IWebPostInputArea InputArea
        {
            get => _inputArea;
            set
            {
                _inputArea = value;
                OnPropertyChanged();
            }
        }
        public ISourceToolRegion<IWebServiceSource> SourceRegion
        {
            get => _sourceRegion;
            set
            {
                _sourceRegion = value;
                InitializeProperties();
                OnPropertyChanged();
            }
        }

        public void ErrorMessage(Exception exception, bool hasError)
        {
            Errors = new List<IActionableErrorInfo>();
            if (hasError)
            {
                Errors = new List<IActionableErrorInfo> { new ActionableErrorInfo(new ErrorInfo() { ErrorType = ErrorType.Critical, FixData = "", FixType = FixType.None, Message = exception.Message, StackTrace = exception.StackTrace }, () => { }) };
            }
        }

        public void ValidateTestComplete()
        {
            OutputsRegion.IsEnabled = true;
        }

        public IWebService ToModel()
        {
            var webServiceDefinition = new WebServiceDefinition
            {
                Inputs = InputsFromModel(),
                OutputMappings = new List<IServiceOutputMapping>(),
                Source = SourceRegion.SelectedSource,
                Name = "",
                Path = "",
                Id = Guid.NewGuid(),
                PostData = InputArea.PostData,
                IsManualChecked = InputArea.IsManualChecked,
                IsFormDataChecked = InputArea.IsFormDataChecked,
                IsUrlEncodedChecked =  InputArea.IsUrlEncodedChecked,
                Headers = InputArea.Headers.Select(value => new NameValue { Name = value.Name, Value = value.Value } as INameValue).ToList(),
                Settings = InputArea.Settings?.Select(value => new NameValue { Name = value.Name, Value = value.Value } as INameValue).ToList(),
                FormDataParameters = BuildFormDataParameters(),
                QueryString = InputArea.QueryString,
                RequestUrl = SourceRegion.SelectedSource.HostName,
                Response = "",
                Method = WebRequestMethod.Post
            };
            return webServiceDefinition;
        }

        private List<IFormDataParameters> BuildFormDataParameters()
        {
            var formDataParameters = new List<IFormDataParameters>();
            foreach (var item in ConditionExpressionOptions.Options)
            {
                if (!(item as FormDataOptionConditionExpression).IsEmptyRow)
                {
                    var expression = new FormDataConditionExpression();
                    expression.FromOption(item);
                    formDataParameters.Add(expression.ToFormDataParameter());
                }
            }
            return formDataParameters;
        }

        IList<IServiceInput> InputsFromModel()
        {
            var dt = new List<IServiceInput>();

            var queryString = InputArea.QueryString;
            _builder.GetValue(queryString, dt);

            foreach (var nameValue in InputArea.Headers)
            {
                _builder.GetValue(nameValue.Name, dt);
                _builder.GetValue(nameValue.Value, dt);
            }

            if (InputArea.IsManualChecked)
            {
                var postValue = InputArea.PostData;
                _builder.GetValue(postValue, dt);
            }

            if (InputArea.IsFormDataChecked)
            {
                foreach (var parameter in BuildFormDataParameters())
                {
                    if (!string.IsNullOrEmpty(parameter.Key))
                    {
                        if (parameter is FileParameter fileParam && !fileParam.IsIncompleteRow)
                        {
                             
                            _builder.GetValue(fileParam.Key, dt);
                            _builder.GetValue(fileParam.FileName, dt);
                            _builder.GetValue(fileParam.FileBase64, dt);
                        }
                        else if (parameter is TextParameter textParam && !textParam.IsIncompleteRow)
                        {
                            _builder.GetValue(textParam.Key, dt);
                            _builder.GetValue(textParam.Value, dt);
                        }
                    }
                }
            }

            return dt;
        }

        IWebServiceModel Model { get; set; }
        public bool GenerateOutputsVisible
        {
            get => _generateOutputsVisible;
            set
            {
                _generateOutputsVisible = value;
                OutputVisibilitySetter.SetGenerateOutputsVisible(ManageServiceInputViewModel.InputArea, ManageServiceInputViewModel.OutputArea, SetRegionVisibility, value);
                OnPropertyChanged();
            }
        }

        public OptionsWithNotifier Options
        {
            get => _options;
            set
            {
                _options = value;
                OnPropertyChanged(nameof(Options));
            }
        }

        void SetRegionVisibility(bool value)
        {
            InputArea.IsEnabled = value;
            OutputsRegion.IsEnabled = value && OutputsRegion.Outputs.Count > 0;
            ErrorRegion.IsEnabled = value;
            SourceRegion.IsEnabled = value;
        }

    }
}

