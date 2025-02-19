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
using System.Collections.Generic;
using System.Linq;
using Dev2.Activities;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Common.State;
using Dev2.Data;
using Dev2.Data.Interfaces;
using Dev2.Data.TO;
using Dev2.DataList.Contract;
using Dev2.Interfaces;
using Dev2.PathOperations;
using Dev2.Util;
using Unlimited.Applications.BusinessDesignStudio.Activities.Utilities;
using Warewolf.Core;
using Warewolf.Storage;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    [ToolDescriptorInfo("FileFolder-Delete", "Delete", ToolType.Native, "8999E59A-38A3-43BB-A98F-6090C5C9EA1E", "Dev2.Activities", "1.0.0.0", "Legacy", "File, FTP, FTPS & SFTP", "/Warewolf.Studio.Themes.Luna;component/Images.xaml", "Tool_File_Delete")]
    public class DsfPathDelete : DsfAbstractFileActivity, IPathInput
    {
        public DsfPathDelete()
            : base("Delete")
        {
            InputPath = string.Empty;
        }

        protected override bool AssignEmptyOutputsToRecordSet => true;
        protected override IList<OutputTO> TryExecuteConcreteAction(IDSFDataObject context, out ErrorResultTO error, int update)
        {
            IList<OutputTO> outputs = new List<OutputTO>();

            error = new ErrorResultTO();

            var colItr = new WarewolfListIterator();

            //get all the possible paths for all the string variables

            var inputItr = new WarewolfIterator(context.Environment.Eval(InputPath, update));
            colItr.AddVariableToIterateOn(inputItr);
            
            var userItr = new WarewolfIterator(context.Environment.Eval(Username,update));
            colItr.AddVariableToIterateOn(userItr);

            var passItr = new WarewolfIterator(context.Environment.Eval(DecryptedPassword,update));
            colItr.AddVariableToIterateOn(passItr);

            var privateKeyItr = new WarewolfIterator(context.Environment.Eval(PrivateKeyFile, update));
            colItr.AddVariableToIterateOn(privateKeyItr);

            outputs.Add(DataListFactory.CreateOutputTO(Result));

            if(context.IsDebugMode())
            {
                AddDebugInputItem(InputPath, "Input Path", context.Environment, update);
                AddDebugInputItemUserNamePassword(context.Environment, update);
                if (!string.IsNullOrEmpty(PrivateKeyFile))
                {
                    AddDebugInputItem(PrivateKeyFile, "Private Key File", context.Environment, update);
                }
            }
            while (colItr.HasMoreData())
            {
                var broker = ActivityIOFactory.CreateOperationsBroker();

                try
                {
                     var dst = ActivityIOFactory.CreatePathFromString(colItr.FetchNextValue(inputItr),
                         colItr.FetchNextValue(userItr),
                         colItr.FetchNextValue(passItr),
                         true, colItr.FetchNextValue(privateKeyItr));

                    var dstEndPoint = ActivityIOFactory.CreateOperationEndPointFromIOPath(dst);

                    var result = broker.Delete(dstEndPoint);
                    outputs[0].OutputStrings.Add(result);
                    outputs.Add(DataListFactory.CreateOutputTO($"Username [ {dstEndPoint.IOPath.Username} ]"));
                }
                catch(Exception e)
                {
                    outputs.Add(DataListFactory.CreateOutputTO(Result, "Failure"));
                    error.AddError(e.Message);
                    break;
                }   
            }

            return outputs;
        }

        /// <summary>
        /// Gets or sets the input path.
        /// </summary>
        [Inputs("Input Path")]
        [FindMissing]
        public string InputPath
        {
            get;
            set;
        }

        public override IEnumerable<StateVariable> GetState()
        {
            return new[] {
                new StateVariable
                {
                    Name = "InputPath",
                    Value = InputPath,
                    Type = StateVariable.StateType.InputOutput
                },
                new StateVariable
                {
                    Name = "Username",
                    Value = Username,
                    Type = StateVariable.StateType.Input
                },
                new StateVariable
                {
                    Name = "PrivateKeyFile",
                    Value = PrivateKeyFile,
                    Type = StateVariable.StateType.Input
                },
                new StateVariable
                {
                    Name="Result",
                    Value = Result,
                    Type = StateVariable.StateType.Output
                }
            };
        }

        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates)
        {
            if(updates != null && updates.Count == 1)
            {
                InputPath = updates[0].Item2;
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

        public override IList<DsfForEachItem> GetForEachInputs() => GetForEachItems(InputPath);

        public override IList<DsfForEachItem> GetForEachOutputs() => GetForEachItems(Result);
    }
}
