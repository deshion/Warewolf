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
using Dev2.Common.ExtMethods;
using Dev2.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Common.Tests.Utils
{
    
    [TestClass]
    public class TextUtilsTests
    {
        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]

        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_ReplaceWackNWithWackRWackN_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "warewolf\r\n is\\n awesome\r\n";
            const string stringToReplace = "warewolf\n is\\n awesome\r\n";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]
        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_WackNFirstChars_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "\r\nwarewolf\r\n is\\n awesome\r\n";
            const string stringToReplace = "\nwarewolf\n is\\n awesome\r\n";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]
        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_WackNSecondChars_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "t\r\nwarewolf\r\n is\\n awesome\r\n";
            const string stringToReplace = "t\nwarewolf\n is\\n awesome\r\n";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]
        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_WackNLastChars_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "t\r\nwarewolf\r\n is\\n awesome\r\n";
            const string stringToReplace = "t\nwarewolf\n is\\n awesome\n";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]
        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_WackWackNFirstChars_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "\\nwarewolf\r\n is\\n awesome\r\ntest";
            const string stringToReplace = "\\nwarewolf\n is\\n awesome\ntest";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines")]
        public void TextUtils_ReplaceWorkflowNewLinesWithEnvironmentNewLines_WackkWackNFirstChars_ReplaceOccurs()
        {
            //------------Setup for test--------------------------
            const string expected = "\\4\\\r\n\r\n\\\\\\\r\n\r\n\\n\r\n";
            const string stringToReplace = "\\4\\\n\n\\\\\\\n\n\\n\n";
            //------------Execute Test---------------------------
            var actual = TextUtils.ReplaceWorkflowNewLinesWithEnvironmentNewLines(stringToReplace);
            //------------Assert Results-------------------------
            Assert.AreEqual(expected, actual, "The replacement didn't work correctly");
        }

        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void ToGui_GivenValidString_ShouldReturnGuid()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var guid = Guid.Empty.ToString().ToGuid();
            var newGuid = Guid.NewGuid().ToString();
            var guid1 = newGuid.ToGuid();

            //---------------Test Result -----------------------
            Assert.AreEqual(Guid.Empty, guid);
            Assert.AreEqual(Guid.Parse(newGuid),guid1 );
        }
    }
}
