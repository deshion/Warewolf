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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Providers.Validation.Rules;
using Dev2.Studio.Core;
using Dev2.Validation;

namespace Dev2.Activities.Designers2.Core
{
    public abstract class FileActivityDesignerViewModel : CredentialsActivityDesignerViewModel, INotifyPropertyChanged
    {
        public static readonly List<string> ValidUriSchemes = new List<string> { "file", "ftp", "ftps", "sftp" };

        protected FileActivityDesignerViewModel(ModelItem modelItem, string inputPathLabel, string outputPathLabel)
            : base(modelItem)
        {
            VerifyArgument.IsNotNull("inputPathLabel", inputPathLabel);
            VerifyArgument.IsNotNull("outputPathLabel", outputPathLabel);
            InputPathLabel = inputPathLabel;
            OutputPathLabel = outputPathLabel;
        }

        public string InputPathLabel { get; private set; }
        public string OutputPathLabel { get; private set; }

        public string InputPathValue { get; private set; }
        public string OutputPathValue { get; set; }

        public string SftpValue { get; private set; }
        public string DestinationSftpValue { get; private set; }

        public string FileContentValue { get; private set; }
        public string ArchivePasswordValue { get; private set; }

        public bool IsInputPathFocused
        {
            get { return (bool)GetValue(IsInputPathFocusedProperty); }
            set
            {
                if (IsInputPathFocusedProperty != null)
                {
                    SetValue(IsInputPathFocusedProperty, value);
                }
            }
        }
        
        public static readonly DependencyProperty IsInputPathFocusedProperty =
            DependencyProperty.Register("IsInputPathFocused", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        public bool IsOutputPathFocused
        {
            get { return (bool)GetValue(IsOutputPathFocusedProperty); }
            set
            {
                if (IsOutputPathFocusedProperty != null)
                {
                    SetValue(IsOutputPathFocusedProperty, value);
                }
            }
        }

        public static readonly DependencyProperty IsOutputPathFocusedProperty =
            DependencyProperty.Register("IsOutputPathFocused", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        public bool IsSftpFocused
        {
            get { return (bool)GetValue(IsSftpFocusedProperty); }
            set
            {
                if (IsSftpFocusedProperty != null)
                {
                    SetValue(IsSftpFocusedProperty, value);
                }
            }
        }

        public static readonly DependencyProperty IsSftpFocusedProperty =
            DependencyProperty.Register("IsSftpFocusedProperty", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        public bool IsDestinationSftpFocused
        {
            get { return (bool)GetValue(IsDestinationSftpFocusedProperty); }
            set
            {
                if (IsDestinationSftpFocusedProperty != null)
                {
                    SetValue(IsDestinationSftpFocusedProperty, value);
                }
            }
        }

        public static readonly DependencyProperty IsDestinationSftpFocusedProperty =
            DependencyProperty.Register("IsDestinationSftpFocusedProperty", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        string InputPath => GetProperty<string>();
        string OutputPath => GetProperty<string>();
        string PrivateKeyFile => GetProperty<string>();
        string DestinationPrivateKeyFile => GetProperty<string>();

        protected virtual void ValidateInputPath() => InputPathValue = ValidatePath(InputPathLabel, InputPath, () => IsInputPathFocused = true, true);

        protected virtual void ValidateOutputPath() => OutputPathValue = ValidatePath(OutputPathLabel, OutputPath, () => IsOutputPathFocused = true, true);

        void ValidateSftpKey() => SftpValue = ValidatePath("Private Key Path", PrivateKeyFile, () => IsSftpFocused = true, false);

        void ValidateDestinationSftpKey() => DestinationSftpValue = ValidatePath("Destination Private Key Path", DestinationPrivateKeyFile, () => IsSftpFocused = true, false);

        protected virtual void ValidateInputAndOutputPaths()
        {
            ValidateOutputPath();
            ValidateInputPath();
            ValidateSftpKey();
            ValidateDestinationSftpKey();
        }

        protected virtual string ValidatePath(string label, string path, Action onError, bool pathIsRequired)
        {
            if (!pathIsRequired && string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            var errors = new List<IActionableErrorInfo>();

            var fileActivityRuleSet = new RuleSet();
            var variableUtils = new VariableUtils();
            var isValidExpressionRule = new IsValidExpressionRule(() => path, DataListSingleton.ActiveDataList.Resource.DataList, variableUtils);
            fileActivityRuleSet.Add(isValidExpressionRule);
            errors.AddRange(fileActivityRuleSet.ValidateRules(label, onError));

            variableUtils.TryParseVariables(path,out string pathValue, onError, variableValue: ValidUriSchemes[0] + "://temp");

            if (errors.Count == 0)
            {
                var isStringEmptyOrWhiteSpaceRuleUserName = new IsStringEmptyOrWhiteSpaceRule(() => path)
                {
                    LabelText = label,
                    DoError = onError
                };

                var isValidFileNameRule = new IsValidFileNameRule(() => path)
                {
                    LabelText = label,
                    DoError = onError
                };

                fileActivityRuleSet.Add(isStringEmptyOrWhiteSpaceRuleUserName);
                fileActivityRuleSet.Add(isValidExpressionRule);
                
                errors.AddRange(fileActivityRuleSet.ValidateRules(label, onError));
            }

            UpdateErrors(errors);
            return pathValue;
        }

        protected virtual void ValidateFileContent(string content, string label) => FileContentValue = ValidateFileContent(content, label, () => FileHasContent = true);

        public bool FileHasContent
        {
            get => (bool)GetValue(FileHasContentProperty);
            set => SetValue(FileHasContentProperty, value);
        }

        public static readonly DependencyProperty FileHasContentProperty = 
            DependencyProperty.Register("FileHasContent", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        protected virtual string ValidateFileContent(string content, string label, Action onError) => ValidateFileContent(content, label, onError, true);

        protected virtual string ValidateFileContent(string content, string label, Action onError, bool contentIsRequired)
        {
            var errors = new List<IActionableErrorInfo>();
            var fileActivityRuleSet = new RuleSet();

            var isValidExpressionRule = new IsValidExpressionRule(() => content, DataListSingleton.ActiveDataList.Resource.DataList,new VariableUtils());
            fileActivityRuleSet.Add(isValidExpressionRule);
            errors.AddRange(fileActivityRuleSet.ValidateRules(label, onError));

            UpdateErrors(errors);
            return content;
        }

        protected virtual void ValidateArchivePassword(string password, string label) => ArchivePasswordValue = ValidateArchivePassword(password, label, () => ArchivePasswordExists = true);

        public bool ArchivePasswordExists
        {
            get => (bool)GetValue(ArchivePasswordExistsProperty);
            set => SetValue(ArchivePasswordExistsProperty, value);
        }

        public static readonly DependencyProperty ArchivePasswordExistsProperty = 
            DependencyProperty.Register("ArchivePasswordExists", typeof(bool), typeof(FileActivityDesignerViewModel), new PropertyMetadata(false));

        protected virtual string ValidateArchivePassword(string password, string label, Action onError) => ValidateArchivePassword(password, label, onError, true);

        protected virtual string ValidateArchivePassword(string password, string label, Action onError, bool contentIsRequired)
        {
            var errors = new List<IActionableErrorInfo>();
            var fileActivityRuleSet = new RuleSet();

            var isValidExpressionRule = new IsValidExpressionRule(() => password, DataListSingleton.ActiveDataList.Resource.DataList, new VariableUtils());
            fileActivityRuleSet.Add(isValidExpressionRule);
            errors.AddRange(fileActivityRuleSet.ValidateRules(label, onError));

            UpdateErrors(errors);
            return password;
        }
        
        protected void OnPropertyChanged() => OnPropertyChanged(null);

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
