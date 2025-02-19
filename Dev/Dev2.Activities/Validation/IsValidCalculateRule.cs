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
using System.Parsing.Intellisense;
using Dev2.Calculate;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Data.Util;
using Dev2.Providers.Errors;
using Dev2.Providers.Validation.Rules;

namespace Dev2.Validation
{
    public class IsValidCalculateRule : Rule<string>
    {
        readonly ISyntaxTreeBuilderHelper _syntaxBuilder;

        public IsValidCalculateRule(Func<string> getValue)
            : base(getValue)
        {
            _syntaxBuilder = new SyntaxTreeBuilderHelper();
        }


        public override IActionableErrorInfo Check()
        {
            var value = GetValue();

            if (DataListUtil.IsCalcEvaluation(value, out string calculationExpression))
            {
                value = calculationExpression;
                _syntaxBuilder.Build(value, false, out Token[] tokens);

                if (_syntaxBuilder.EventLog != null && _syntaxBuilder.HasEventLogs)
                {

                    return new ActionableErrorInfo(DoError) { Message = "Syntax Error An error occurred while parsing { " + value + " } It appears to be malformed" };
                }
            }

            return null;
        }
    }
}
