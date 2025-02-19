#pragma warning disable
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
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using Dev2.Activities.Debug;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Data;
using Dev2.Data.Interfaces.Enums;
using Dev2.Data.TO;
using Dev2.Data.Util;
using Dev2.DataList.Contract;
using Dev2.Diagnostics;
using Dev2.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Util;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Core;
using Warewolf.Resource.Errors;
using Warewolf.Security.Encryption;
using Warewolf.Storage;
using Warewolf.Storage.Interfaces;
using Dev2.Comparer;
using Dev2.Common.State;

namespace Dev2.Activities
{
    [ToolDescriptorInfo("Utility-SendMail", "SMTP Send", ToolType.Native, "8999E59A-38A3-43BB-A98F-6090C5C9EA1E", "Dev2.Activities", "1.0.0.0", "Legacy", "Email", "/Warewolf.Studio.Themes.Luna;component/Images.xaml", "Tool_Email_SMTP_Send")]
    public class DsfSendEmailActivity : DsfActivityAbstract<string>,IEquatable<DsfSendEmailActivity>
    {
        IEmailSender _emailSender;
        IDSFDataObject _dataObject;
        string _password;
        EmailSource _selectedEmailSource;
                
        public EmailSource SelectedEmailSource
        {
            get => _selectedEmailSource;
            set
            {
                _selectedEmailSource = value;
                if(_selectedEmailSource != null)
                {
                    var resourceID = _selectedEmailSource.ResourceID;
                    _selectedEmailSource = null;
                    _selectedEmailSource = new EmailSource { ResourceID = resourceID };
                }
            }
        }
        
        [FindMissing]
        public string FromAccount { get; set; }
        
        [FindMissing]
        public string Password
        {
            get => _password;
            set
            {
                if (DataListUtil.ShouldEncrypt(value))
                {
                    try
                    {
                        _password = DpapiWrapper.Encrypt(value);
                    }
                    catch (Exception)
                    {
                        _password = value;
                    }
                }
                else
                {
                    _password = value;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected string DecryptedPassword => DataListUtil.NotEncrypted(Password) ? Password : DpapiWrapper.Decrypt(Password);

        [FindMissing]
        public string To { get; set; }
        [FindMissing]
        public string Cc { get; set; }
        [FindMissing]
        public string Bcc { get; set; }
        
        public enMailPriorityEnum Priority { get; set; }
        
        [FindMissing]
        public string Subject { get; set; }
        [FindMissing]
        public string Attachments { get; set; }
        [FindMissing]
        public string Body { get; set; }
        
        public bool IsHtml { get; set; }
        
        [FindMissing]
        public new string Result { get; set; }

        public IEmailSender EmailSender
        {
            get => _emailSender ?? (_emailSender = new EmailSender());
            set
            {
                _emailSender = value;
            }
        }

        public override IEnumerable<StateVariable> GetState()
        {
            return new[] {
                new StateVariable
                {
                    Name = "SelectedEmailSource.ResourceID",
                    Type = StateVariable.StateType.Input,
                    Value = SelectedEmailSource.ResourceID.ToString()
                },
                new StateVariable
                {
                    Name = "FromAccount",
                    Type = StateVariable.StateType.Input,
                    Value = FromAccount
                },
                new StateVariable
                {
                    Name = "To",
                    Type = StateVariable.StateType.Input,
                    Value = To
                },
                new StateVariable
                {
                    Name = "Cc",
                    Type = StateVariable.StateType.Input,
                    Value = Cc
                },
                new StateVariable
                {
                    Name = "Bcc",
                    Type = StateVariable.StateType.Input,
                    Value = Bcc
                },
                new StateVariable
                {
                    Name = "Priority",
                    Type = StateVariable.StateType.Input,
                    Value = Priority.ToString()
                },
                new StateVariable
                {
                    Name = "Subject",
                    Type = StateVariable.StateType.Input,
                    Value = Subject
                },
                new StateVariable
                {
                    Name = "Attachments",
                    Type = StateVariable.StateType.Input,
                    Value = Attachments
                },
                new StateVariable
                {
                    Name = "IsHtml",
                    Type = StateVariable.StateType.Input,
                    Value = IsHtml.ToString()
                },
                new StateVariable
                {
                    Name = "Body",
                    Type = StateVariable.StateType.Input,
                    Value = Body
                },
                new StateVariable
                {
                    Name = "Result",
                    Type = StateVariable.StateType.Output,
                    Value = Result
                }
            };
        }

        public DsfSendEmailActivity()
            : base("Email")
        {
            FromAccount = string.Empty;
            To = string.Empty;
            Cc = string.Empty;
            Bcc = string.Empty;
            Password = string.Empty;
            Priority = enMailPriorityEnum.Normal;
            Subject = string.Empty;
            Attachments = string.Empty;
            Body = string.Empty;
            IsHtml = false;
        }

        public override List<string> GetOutputs() => new List<string> { Result };

        bool IsDebug
        {
            get
            {
                if (_dataObject == null)
                {
                    return false;
                }
                return _dataObject.IsDebugMode();
            }
        }

        protected override void OnExecute(NativeActivityContext context)
        {
            var dataObject = context.GetExtension<IDSFDataObject>();
            ExecuteTool(dataObject, 0);
        }

        protected override void ExecuteTool(IDSFDataObject dataObject, int update)
        {
            _dataObject = dataObject;

            var allErrors = new ErrorResultTO();
            var indexToUpsertTo = 0;

            InitializeDebug(dataObject);
            try
            {
                var runtimeSource = ResourceCatalog.GetResource<EmailSource>(dataObject.WorkspaceID, SelectedEmailSource.ResourceID);

                if (runtimeSource == null)
                {
                    dataObject.Environment.Errors.Add(ErrorResource.InvalidEmailSource);
                    return;
                }
                indexToUpsertTo = TryExecute(dataObject, update, allErrors, indexToUpsertTo, runtimeSource);
            }
            catch (Exception e)
            {
                Dev2Logger.Error("DSFEmail", e, GlobalConstants.WarewolfError);
                allErrors.AddError(e.Message);
            }

            finally
            {
                // Handle Errors
                if(allErrors.HasErrors())
                {
                    foreach(var err in allErrors.FetchErrors())
                    {
                        dataObject.Environment.Errors.Add(err);
                    }
                    UpsertResult(indexToUpsertTo, dataObject.Environment, null, update);
                    if(dataObject.IsDebugMode())
                    {
                        AddDebugOutputItem(new DebugItemStaticDataParams("", Result, ""));
                    }
                    DisplayAndWriteError(dataObject,DisplayName, allErrors);
                }
                if(dataObject.IsDebugMode())
                {
                    DispatchDebugState(dataObject, StateType.Before, update);
                    DispatchDebugState(dataObject, StateType.After, update);
                }
            }
        }

#pragma warning disable S1541 // Methods and properties should not be too complex
        private int TryExecute(IDSFDataObject dataObject, int update, ErrorResultTO allErrors, int indexToUpsertTo, EmailSource runtimeSource)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            if (IsDebug)
            {
                var fromAccount = FromAccount;
                if (String.IsNullOrEmpty(fromAccount))
                {
                    fromAccount = runtimeSource.UserName;
                    AddDebugInputItem(fromAccount, "From Account");
                }
                else
                {
                    AddDebugInputItem(new DebugEvalResult(FromAccount, "From Account", dataObject.Environment, update));
                }
                AddDebugInputItem(new DebugEvalResult(To, "To", dataObject.Environment, update));
                AddDebugInputItem(new DebugEvalResult(Subject, "Subject", dataObject.Environment, update));
                AddDebugInputItem(new DebugEvalResult(Body, "Body", dataObject.Environment, update));
            }
            var colItr = new WarewolfListIterator();

            var fromAccountItr = new WarewolfIterator(dataObject.Environment.Eval(FromAccount ?? string.Empty, update));
            colItr.AddVariableToIterateOn(fromAccountItr);

            var passwordItr = new WarewolfIterator(dataObject.Environment.Eval(DecryptedPassword, update));
            colItr.AddVariableToIterateOn(passwordItr);

            var toItr = new WarewolfIterator(dataObject.Environment.Eval(To, update));
            colItr.AddVariableToIterateOn(toItr);

            var ccItr = new WarewolfIterator(dataObject.Environment.Eval(Cc, update));
            colItr.AddVariableToIterateOn(ccItr);

            var bccItr = new WarewolfIterator(dataObject.Environment.Eval(Bcc, update));
            colItr.AddVariableToIterateOn(bccItr);

            var subjectItr = new WarewolfIterator(dataObject.Environment.Eval(Subject, update));
            colItr.AddVariableToIterateOn(subjectItr);

            var bodyItr = new WarewolfIterator(dataObject.Environment.Eval(Body ?? string.Empty, update));
            colItr.AddVariableToIterateOn(bodyItr);

            var attachmentsItr = new WarewolfIterator(dataObject.Environment.Eval(Attachments ?? string.Empty, update));
            colItr.AddVariableToIterateOn(attachmentsItr);

            if (!allErrors.HasErrors())
            {
                while (colItr.HasMoreData())
                {
                    var result = SendEmail(runtimeSource, colItr, fromAccountItr, passwordItr, toItr, ccItr, bccItr, subjectItr, bodyItr, attachmentsItr, out ErrorResultTO errors);
                    allErrors.MergeErrors(errors);
                    if (!allErrors.HasErrors())
                    {
                        indexToUpsertTo = UpsertResult(indexToUpsertTo, dataObject.Environment, result, update);
                    }
                }
                if (IsDebug && !allErrors.HasErrors() && !string.IsNullOrEmpty(Result))
                {
                    AddDebugOutputItem(new DebugEvalResult(Result, "", dataObject.Environment, update));
                }
            }
            else
            {
                if (IsDebug)
                {
                    AddDebugInputItem(FromAccount, "From Account");
                    AddDebugInputItem(To, "To");
                    AddDebugInputItem(Subject, "Subject");
                    AddDebugInputItem(Body, "Body");
                }
            }

            return indexToUpsertTo;
        }

        void AddDebugInputItem(string value, string label)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrEmpty(label))
            {
                return;
            }

            AddDebugInputItem(DataListUtil.IsEvaluated(value) ? new DebugItemStaticDataParams("", value, label) : new DebugItemStaticDataParams(value, label));
        }

        int UpsertResult(int indexToUpsertTo, IExecutionEnvironment environment, string result, int update)
        {
            string expression;
            expression = DataListUtil.IsValueRecordset(Result) && DataListUtil.GetRecordsetIndexType(Result) == enRecordsetIndexType.Star ? Result.Replace(GlobalConstants.StarExpression, indexToUpsertTo.ToString(CultureInfo.InvariantCulture)) : Result;
            foreach (var region in DataListCleaningUtils.SplitIntoRegions(expression))
            {
                environment.Assign(region, result, update);
                indexToUpsertTo++;
            }
            return indexToUpsertTo;
        }

        string SendEmail(EmailSource runtimeSource, IWarewolfListIterator colItr, IWarewolfIterator fromAccountItr, IWarewolfIterator passwordItr, IWarewolfIterator toItr, IWarewolfIterator ccItr, IWarewolfIterator bccItr, IWarewolfIterator subjectItr, IWarewolfIterator bodyItr, IWarewolfIterator attachmentsItr, out ErrorResultTO errors)
        {
            errors = new ErrorResultTO();
            var fromAccountValue = colItr.FetchNextValue(fromAccountItr);
            var passwordValue = colItr.FetchNextValue(passwordItr);
            var toValue = colItr.FetchNextValue(toItr);
            var ccValue = colItr.FetchNextValue(ccItr);
            var bccValue = colItr.FetchNextValue(bccItr);
            var subjectValue = colItr.FetchNextValue(subjectItr);
            var bodyValue = colItr.FetchNextValue(bodyItr);
            var attachmentsValue = colItr.FetchNextValue(attachmentsItr);
            var mailMessage = new MailMessage { IsBodyHtml = IsHtml };
            if (Enum.TryParse(Priority.ToString(), true, out MailPriority priority))
            {
                mailMessage.Priority = priority;
            }
            mailMessage.Subject = subjectValue;
            AddToAddresses(toValue, mailMessage);
            try
            {
                // Always use source account unless specifically overridden by From Account
                if(!string.IsNullOrEmpty(fromAccountValue))
                {
                    runtimeSource.UserName = fromAccountValue;
                    runtimeSource.Password = passwordValue;
                }
                mailMessage.From = new MailAddress(runtimeSource.UserName);
            }
            catch(Exception)
            {
                errors.AddError(string.Format(ErrorResource.FROMAddressInvalidFormat, fromAccountValue));
                return "Failure";
            }
            mailMessage.Body = bodyValue;
            if(!String.IsNullOrEmpty(ccValue))
            {
                AddCcAddresses(ccValue, mailMessage);
            }
            if(!String.IsNullOrEmpty(bccValue))
            {
                AddBccAddresses(bccValue, mailMessage);
            }
            if(!String.IsNullOrEmpty(attachmentsValue))
            {
                AddAttachmentsValue(attachmentsValue, mailMessage);
            }
            string result;
            try
            {
                EmailSender.Send(runtimeSource, mailMessage);
                result = "Success";
            }
            catch(Exception e)
            {
                result = "Failure";
                errors.AddError(e.Message);
            }

            return result;
        }

        List<string> GetSplitValues(string stringToSplit, char[] splitOn) => stringToSplit.Split(splitOn, StringSplitOptions.RemoveEmptyEntries).ToList();

        void AddAttachmentsValue(string attachmentsValue, MailMessage mailMessage)
        {
            try
            {
                var attachements = GetSplitValues(attachmentsValue, new[] { ',', ';' });
                attachements.ForEach(s => mailMessage.Attachments.Add(new Attachment(s)));
            }
            catch(Exception exception)
            {
                throw new Exception(string.Format(ErrorResource.AttachmentInvalidFormat, attachmentsValue), exception);
            }
        }

        void AddToAddresses(string toValue, MailMessage mailMessage)
        {
            try
            {
                var toAddresses = GetSplitValues(toValue, new[] { ',', ';' });
                toAddresses.ForEach(s => mailMessage.To.Add(new MailAddress(s)));
            }
            catch(FormatException exception)
            {
                throw new Exception(string.Format(ErrorResource.ToAddressInvalidFormat, toValue), exception);
            }
        }

        void AddCcAddresses(string toValue, MailMessage mailMessage)
        {
            try
            {
                var ccAddresses = GetSplitValues(toValue, new[] { ',', ';' });
                ccAddresses.ForEach(s => mailMessage.CC.Add(new MailAddress(s)));
            }
            catch(FormatException exception)
            {
                throw new Exception(string.Format(ErrorResource.CCAddressInvalidFormat, toValue), exception);
            }
        }

        void AddBccAddresses(string toValue, MailMessage mailMessage)
        {
            try
            {
                var bccAddresses = GetSplitValues(toValue, new[] { ',', ';' });
                bccAddresses.ForEach(s => mailMessage.Bcc.Add(new MailAddress(s)));
            }
            catch(FormatException exception)
            {
                throw new Exception(string.Format(ErrorResource.BCCAddressInvalidFormat, toValue), exception);
            }
        }

        public override enFindMissingType GetFindMissingType() => enFindMissingType.StaticActivity;

#pragma warning disable S1541 // Methods and properties should not be too complex
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            if(updates != null)
            {
                foreach(Tuple<string, string> t in updates)
                {

                    if(t.Item1 == FromAccount)
                    {
                        FromAccount = t.Item2;
                    }
                    if(t.Item1 == Password)
                    {
                        Password = t.Item2;
                    }
                    if(t.Item1 == To)
                    {
                        To = t.Item2;
                    }
                    if(t.Item1 == Cc)
                    {
                        Cc = t.Item2;
                    }
                    if(t.Item1 == Bcc)
                    {
                        Bcc = t.Item2;
                    }
                    if(t.Item1 == Subject)
                    {
                        Subject = t.Item2;
                    }
                    if(t.Item1 == Attachments)
                    {
                        Attachments = t.Item2;
                    }
                    if(t.Item1 == Body)
                    {
                        Body = t.Item2;
                    }
                }
            }
        }

        public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates)
        {
            var itemUpdate = updates?.FirstOrDefault(tuple => tuple.Item1 == Result);
            if(itemUpdate != null)
            {
                Result = itemUpdate.Item2;
            }
        }

        public override List<DebugItem> GetDebugInputs(IExecutionEnvironment env, int update)
        {
            foreach(IDebugItem debugInput in _debugInputs)
            {
                debugInput.FlushStringBuilder();
            }
            return _debugInputs;
        }

        public override List<DebugItem> GetDebugOutputs(IExecutionEnvironment env, int update)
        {
            foreach(IDebugItem debugOutput in _debugOutputs)
            {
                debugOutput.FlushStringBuilder();
            }
            return _debugOutputs;
        }

        public override IList<DsfForEachItem> GetForEachInputs() => GetForEachItems(FromAccount, Password, To, Cc, Bcc, Subject, Attachments, Body);

        public override IList<DsfForEachItem> GetForEachOutputs() => GetForEachItems(Result);

#pragma warning disable S1541 // Methods and properties should not be too complex
        public bool Equals(DsfSendEmailActivity other)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var emailSourcesComparer = new EmailSourceComparer();
            var emailSourcesAreEqual = emailSourcesComparer.Equals(SelectedEmailSource, other.SelectedEmailSource);
            var paswordsAreEqual = CommonEqualityOps.PassWordsCompare(Password, other.Password);
            return base.Equals(other) 
                && paswordsAreEqual
                && emailSourcesAreEqual
                && string.Equals(FromAccount, other.FromAccount) 
                && string.Equals(To, other.To) 
                && string.Equals(Cc, other.Cc) 
                && string.Equals(Bcc, other.Bcc) 
                && Priority == other.Priority 
                && string.Equals(Subject, other.Subject) 
                && string.Equals(Attachments, other.Attachments) 
                && string.Equals(Body, other.Body) 
                && IsHtml == other.IsHtml 
                && string.Equals(Result, other.Result);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((DsfSendEmailActivity) obj);
        }

#pragma warning disable S1541 // Methods and properties should not be too complex
        public override int GetHashCode()
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_emailSender != null ? _emailSender.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_dataObject != null ? _dataObject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_password != null ? _password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_selectedEmailSource != null ? _selectedEmailSource.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FromAccount != null ? FromAccount.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Cc != null ? Cc.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Bcc != null ? Bcc.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Priority;
                hashCode = (hashCode * 397) ^ (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Attachments != null ? Attachments.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsHtml.GetHashCode();
                hashCode = (hashCode * 397) ^ (Result != null ? Result.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
