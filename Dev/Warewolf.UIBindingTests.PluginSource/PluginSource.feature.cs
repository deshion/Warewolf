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
namespace Warewolf.UIBindingTests.PluginSource
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class PluginSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = new string[] {
                "CannotParallelize",
                "PluginSource"};
        
#line 1 "PluginSource.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "", "PluginSource", "\tIn order to create plugins\r\n\tAs a Warewolf User\r\n\tI want to be able to select dl" +
                    "ls as a source to be used", ProgrammingLanguage.CSharp, new string[] {
                        "CannotParallelize",
                        "PluginSource"});
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "PluginSource")))
            {
                global::Warewolf.UIBindingTests.PluginSource.PluginSourceFeature.FeatureSetup(null);
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
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("New Plugin Source File")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CannotParallelize")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void NewPluginSourceFile()
        {
            string[] tagsOfScenario = new string[] {
                    "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                        "1.dll",
                    "MSTest:DeploymentItem:Warewolf_Studio.exe",
                    "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                    "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                    "MSTest:DeploymentItem:System.Windows.Interactivity.dll"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("New Plugin Source File", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 18
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
#line 19
 testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 20
 testRunner.Then("\"New DotNet Plugin Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 21
 testRunner.And("title is \"New DotNet Plugin Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 22
 testRunner.And("ConfigFile textbox is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 23
 testRunner.And("ConfigFileButton button is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 24
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 25
 testRunner.When("I type \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Testing.Fakes" +
                        ".dll\" in \"AssemblyName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 26
 testRunner.Then("ConfigFile textbox is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 27
 testRunner.Then("ConfigFileButton button is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 28
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 29
 testRunner.When("I type \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Testing.Fakes" +
                        ".dl\" in \"AssemblyName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 30
 testRunner.Then("ConfigFile textbox is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 31
 testRunner.Then("ConfigFileButton button is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 32
 testRunner.Then("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 33
 testRunner.When("I type \"C:\\Development\\Dev\\Binaries\\MS Fakes\\Microsoft.QualityTools.Testing.Fakes" +
                        ".dll\" in \"AssemblyName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 34
 testRunner.Then("ConfigFile textbox is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 35
 testRunner.Then("ConfigFileButton button is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 36
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 37
 testRunner.When("I save as \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 38
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 39
 testRunner.Then("title is \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 40
 testRunner.And("\"Testing Resource Save\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("New Plugin Source GAC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CannotParallelize")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void NewPluginSourceGAC()
        {
            string[] tagsOfScenario = new string[] {
                    "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                        "1.dll",
                    "MSTest:DeploymentItem:Warewolf_Studio.exe",
                    "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                    "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                    "MSTest:DeploymentItem:System.Windows.Interactivity.dll"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("New Plugin Source GAC", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 48
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
#line 49
testRunner.Given("I open New Plugin Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 50
 testRunner.Then("\"New DotNet Plugin Source\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 51
 testRunner.And("title is \"New DotNet Plugin Source\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 52
 testRunner.And("ConfigFile textbox is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 53
 testRunner.And("ConfigFileButton button is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
 testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 55
 testRunner.When("I type \"GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutral, PublicKe" +
                        "yToken=b03f5f7f11d50a3a.dll\" in \"GacAssemblyName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
 testRunner.Then("ConfigFile textbox is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 57
 testRunner.Then("ConfigFileButton button is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 58
 testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 59
 testRunner.When("I save as \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 61
 testRunner.Then("title is \"Testing Resource Save\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 62
 testRunner.And("\"Testing Resource Save\" tab is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing Plugin Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "PluginSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CannotParallelize")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PluginSource")]
        public virtual void EditingPluginSource()
        {
            string[] tagsOfScenario = new string[] {
                    "MSTest:DeploymentItem:InfragisticsWPF4.Controls.Interactions.XamDialogWindow.v15." +
                        "1.dll",
                    "MSTest:DeploymentItem:Warewolf_Studio.exe",
                    "MSTest:DeploymentItem:Newtonsoft.Json.dll",
                    "MSTest:DeploymentItem:Microsoft.Practices.Prism.SharedInterfaces.dll",
                    "MSTest:DeploymentItem:System.Windows.Interactivity.dll"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing Plugin Source", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
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
#line 71
 testRunner.Given("I open \"Test\" plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 72
 testRunner.Then("title is \"Test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 73
 testRunner.And("\"GacAssemblyName\" value is \"GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Cultu" +
                        "re=neutral, PublicKeyToken=b03f5f7f11d50a3a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.When("I type \"GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutral, PublicKe" +
                        "yToken=b03f5f7f11d50a3\" in \"GacAssemblyName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 75
 testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 76
 testRunner.When("I save Plugin source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
