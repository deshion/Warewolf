﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dev2.Activities.Specs.StudioTestFramework
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class StudioTestFrameworkWithDeletedResourcesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = new string[] {
                "StudioTestFrameworkWithDeletedResources"};
        
#line 1 "StudioTestFrameworkWithDeletedResources.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "StudioTestFramework", "StudioTestFrameworkWithDeletedResources", "\tIn order to test workflows after I have deleted something in warewolf \r\n\tAs a us" +
                    "er\r\n\tI want to create, edit, delete and update tests in a test window", ProgrammingLanguage.CSharp, new string[] {
                        "StudioTestFrameworkWithDeletedResources"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "StudioTestFrameworkWithDeletedResources")))
            {
                global::Dev2.Activities.Specs.StudioTestFramework.StudioTestFrameworkWithDeletedResourcesFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 8
#line hidden
#line 9
  testRunner.Given("test folder is cleaned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1064 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input Var Name"});
            table1064.AddRow(new string[] {
                        "[[a]]"});
#line 10
  testRunner.And("I have \"Workflow 1\" with inputs as", ((string)(null)), table1064, "And ");
#line hidden
            TechTalk.SpecFlow.Table table1065 = new TechTalk.SpecFlow.Table(new string[] {
                        "Ouput Var Name"});
            table1065.AddRow(new string[] {
                        "[[outputValue]]"});
#line 13
  testRunner.And("\"Workflow 1\" has outputs as", ((string)(null)), table1065, "And ");
#line hidden
            TechTalk.SpecFlow.Table table1066 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input Var Name"});
            table1066.AddRow(new string[] {
                        "[[rec().a]]"});
            table1066.AddRow(new string[] {
                        "[[rec().b]]"});
#line 16
  testRunner.Given("I have \"Workflow 2\" with inputs as", ((string)(null)), table1066, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1067 = new TechTalk.SpecFlow.Table(new string[] {
                        "Ouput Var Name"});
            table1067.AddRow(new string[] {
                        "[[returnVal]]"});
#line 20
  testRunner.And("\"Workflow 2\" has outputs as", ((string)(null)), table1067, "And ");
#line hidden
            TechTalk.SpecFlow.Table table1068 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input Var Name"});
            table1068.AddRow(new string[] {
                        "[[A]]"});
            table1068.AddRow(new string[] {
                        "[[B]]"});
            table1068.AddRow(new string[] {
                        "[[C]]"});
#line 23
  testRunner.Given("I have \"Workflow 3\" with inputs as", ((string)(null)), table1068, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1069 = new TechTalk.SpecFlow.Table(new string[] {
                        "Ouput Var Name"});
            table1069.AddRow(new string[] {
                        "[[message]]"});
#line 28
  testRunner.And("\"Workflow 3\" has outputs as", ((string)(null)), table1069, "And ");
#line hidden
            TechTalk.SpecFlow.Table table1070 = new TechTalk.SpecFlow.Table(new string[] {
                        "Input Var Name"});
            table1070.AddRow(new string[] {
                        "[[input]]"});
#line 31
  testRunner.Given("I have \"WorkflowWithTests\" with inputs as", ((string)(null)), table1070, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1071 = new TechTalk.SpecFlow.Table(new string[] {
                        "Ouput Var Name"});
            table1071.AddRow(new string[] {
                        "[[outputValue]]"});
#line 34
  testRunner.And("\"WorkflowWithTests\" has outputs as", ((string)(null)), table1071, "And ");
#line hidden
            TechTalk.SpecFlow.Table table1072 = new TechTalk.SpecFlow.Table(new string[] {
                        "TestName",
                        "AuthenticationType",
                        "Error",
                        "TestFailing",
                        "TestPending",
                        "TestInvalid",
                        "TestPassed"});
            table1072.AddRow(new string[] {
                        "Test1",
                        "Windows",
                        "false",
                        "false",
                        "false",
                        "false",
                        "true"});
            table1072.AddRow(new string[] {
                        "Test2",
                        "Windows",
                        "false",
                        "true",
                        "false",
                        "false",
                        "false"});
            table1072.AddRow(new string[] {
                        "Test3",
                        "Windows",
                        "false",
                        "false",
                        "false",
                        "true",
                        "false"});
            table1072.AddRow(new string[] {
                        "Test4",
                        "Windows",
                        "false",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 37
  testRunner.And("\"WorkflowWithTests\" Tests as", ((string)(null)), table1072, "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete an Disabled Test")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void DeleteAnDisabledTest()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete an Disabled Test", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 44
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 45
 testRunner.Given("the test builder is open with \"Workflow 3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 46
 testRunner.And("Tab Header is \"Workflow 3 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 47
 testRunner.And("there are no tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 48
 testRunner.And("I click New Test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1073 = new TechTalk.SpecFlow.Table(new string[] {
                            "TestName",
                            "AuthenticationType",
                            "Error"});
                table1073.AddRow(new string[] {
                            "Test1",
                            "Windows",
                            "true"});
#line 49
 testRunner.And("I set Test Values as", ((string)(null)), table1073, "And ");
#line hidden
#line 52
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 53
 testRunner.Then("Tab Header is \"Workflow 3 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 54
 testRunner.And("there are 1 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 55
 testRunner.When("I enable \"Test1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
 testRunner.Then("Delete is disabled for \"Test1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 57
 testRunner.When("I disable \"Test1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 58
 testRunner.Then("Delete is enabled for \"Test1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 59
 testRunner.When("I delete \"Test1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
 testRunner.Then("The \"DeleteConfirmation\" popup is shown I click Ok", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 61
 testRunner.Then("there are 0 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete Resource with tests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void DeleteResourceWithTests()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete Resource with tests", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 63
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 64
 testRunner.Given("I have a resouce \"PluginSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 65
 testRunner.And("I add \"test1,test2,test3\" as tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 66
 testRunner.Then("\"PluginSource\" has 3 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 67
 testRunner.When("I delete resource \"PluginSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
 testRunner.Then("\"PluginSource\" has 0 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete folder with resources deletes all tests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void DeleteFolderWithResourcesDeletesAllTests()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete folder with resources deletes all tests", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 70
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 71
 testRunner.Given("I have a folder \"Home\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 72
 testRunner.And("I have a resouce workflow \"PluginSource1\" inside Home", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.And("I add \"test 1,test 2,test 3\" to \"PluginSource1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.And("I have a resouce workflow \"Hello bob\" inside Home", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.And("I add \"test 4,test 5,test 6\" to \"Hello bob\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 76
 testRunner.Then("\"PluginSource1\" has 3 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 77
 testRunner.And("\"Hello bob\" has 3 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 78
 testRunner.When("I delete folder \"Home\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 79
 testRunner.Then("\"PluginSource1\" has 0 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 80
 testRunner.And("\"Hello bob\" has 0 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Save a New Test fails when workflow deleted")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void SaveANewTestFailsWhenWorkflowDeleted()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Save a New Test fails when workflow deleted", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 82
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 83
 testRunner.Given("the test builder is open with \"Workflow 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 84
 testRunner.And("Tab Header is \"Workflow 1 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 85
 testRunner.And("there are no tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 86
 testRunner.And("I click New Test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 87
 testRunner.Then("a new test is added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 88
 testRunner.And("Tab Header is \"Workflow 1 - Tests *\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 89
 testRunner.And("test name starts with \"Test 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1074 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1074.AddRow(new string[] {
                            "a",
                            ""});
#line 90
 testRunner.And("inputs are", ((string)(null)), table1074, "And ");
#line hidden
                TechTalk.SpecFlow.Table table1075 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1075.AddRow(new string[] {
                            "outputValue",
                            ""});
#line 93
 testRunner.And("outputs as", ((string)(null)), table1075, "And ");
#line hidden
#line 96
 testRunner.And("save is enabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 97
 testRunner.When("\"Workflow 1\" is deleted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 98
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 99
 testRunner.Then("The \"Workflow Deleted\" popup is shown I click Ok", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 100
 testRunner.And("the tab is closed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Run Selected Test fails when workflow deleted")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void RunSelectedTestFailsWhenWorkflowDeleted()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Run Selected Test fails when workflow deleted", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 102
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 103
 testRunner.Given("the test builder is open with \"Workflow 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 104
 testRunner.And("Tab Header is \"Workflow 1 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("there are no tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
 testRunner.And("I click New Test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 107
 testRunner.Then("a new test is added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 108
 testRunner.And("Tab Header is \"Workflow 1 - Tests *\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
 testRunner.And("test name starts with \"Test 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1076 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1076.AddRow(new string[] {
                            "a",
                            ""});
#line 110
 testRunner.And("inputs are", ((string)(null)), table1076, "And ");
#line hidden
                TechTalk.SpecFlow.Table table1077 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1077.AddRow(new string[] {
                            "outputValue",
                            ""});
#line 113
 testRunner.And("outputs as", ((string)(null)), table1077, "And ");
#line hidden
#line 116
 testRunner.And("save is enabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 117
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 118
 testRunner.Then("Tab Header is \"Workflow 1 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 119
 testRunner.And("I close the test builder", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 120
 testRunner.When("the test builder is open with \"Workflow 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 121
 testRunner.Then("there are 1 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 122
 testRunner.And("\"Dummy Test\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 123
 testRunner.And("I select \"Test 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 124
 testRunner.And("\"Test 1\" is selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 125
 testRunner.And("test name starts with \"Test 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1078 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1078.AddRow(new string[] {
                            "a",
                            ""});
#line 126
 testRunner.And("inputs are", ((string)(null)), table1078, "And ");
#line hidden
                TechTalk.SpecFlow.Table table1079 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1079.AddRow(new string[] {
                            "outputValue",
                            ""});
#line 129
 testRunner.And("outputs as", ((string)(null)), table1079, "And ");
#line hidden
#line 132
 testRunner.And("save is disabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 133
 testRunner.When("\"Workflow 1\" is deleted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 134
 testRunner.And("I run selected test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 135
 testRunner.Then("The \"Workflow Deleted\" popup is shown I click Ok", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 136
 testRunner.And("the tab is closed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Run All Tests fails when workflow deleted")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "StudioTestFrameworkWithDeletedResources")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("StudioTestFrameworkWithDeletedResources")]
        public virtual void RunAllTestsFailsWhenWorkflowDeleted()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Run All Tests fails when workflow deleted", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 138
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 8
this.FeatureBackground();
#line hidden
#line 139
 testRunner.Given("the test builder is open with \"Workflow 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 140
 testRunner.And("Tab Header is \"Workflow 1 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 141
 testRunner.And("there are no tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 142
 testRunner.And("I click New Test", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 143
 testRunner.Then("a new test is added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 144
 testRunner.And("Tab Header is \"Workflow 1 - Tests *\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 145
 testRunner.And("test name starts with \"Test 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table1080 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1080.AddRow(new string[] {
                            "a",
                            ""});
#line 146
 testRunner.And("inputs are", ((string)(null)), table1080, "And ");
#line hidden
                TechTalk.SpecFlow.Table table1081 = new TechTalk.SpecFlow.Table(new string[] {
                            "Variable Name",
                            "Value"});
                table1081.AddRow(new string[] {
                            "outputValue",
                            ""});
#line 149
 testRunner.And("outputs as", ((string)(null)), table1081, "And ");
#line hidden
#line 152
 testRunner.And("save is enabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 153
 testRunner.When("I save", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 154
 testRunner.Then("Tab Header is \"Workflow 1 - Tests\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 155
 testRunner.And("I close the test builder", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 156
 testRunner.When("the test builder is open with \"Workflow 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 157
 testRunner.Then("there are 1 tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 158
 testRunner.When("\"Workflow 1\" is deleted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 159
 testRunner.And("I run all tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 160
 testRunner.Then("The \"Workflow Deleted\" popup is shown I click Ok", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 161
 testRunner.And("the tab is closed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
