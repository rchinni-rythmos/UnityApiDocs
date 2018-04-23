using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class XmlDocHandlerTest : XmlDocHandlerTestBase
    {
        [Test]
        public void GetTypes_Returns_Relative_Path_When_Given_Path_Without_Trailing_Slash()
        {
            var testFileDirectory = "TestTypes/CommonTypes";
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

            Assert.That(xmlActual, Contains.Substring("<path>AClass.cs</path>"));
            Assert.That(xmlActual, Contains.Substring(@"<path>AFolder\AClass.part2.cs</path>"));
        }

        [Test]
        public void GetTypes_Full_ReturnsCorrectXml()
        {
            var testFileDirectory = "TestTypes/CommonTypes/";
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

            var expected = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""1"">
    <types>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</id>
            <parentId></parentId>
            <name>AClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>
<path>AFolder\AClass.part2.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.INestedInterface</id>
            <parentId>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</parentId>
            <name>INestedInterface</name>
            <kind>Interface</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.Delegate</id>
            <parentId>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</parentId>
            <name>Delegate</name>
            <kind>Delegate</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>
            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AnEnum</id>
            <parentId></parentId>
            <name>AnEnum</name>
            <kind>Enum</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AnEnum.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.ADelegate</id>
            <parentId></parentId>
            <name>ADelegate</name>
            <kind>Delegate</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>GlobalDelegate.cs</path>
            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.AnEnum</id>
            <parentId>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</parentId>
            <name>AnEnum</name>
            <kind>Enum</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AFolder\AClass.part2.cs</path>

            </relativeFilePaths>
        </type></types></doc>";

            AssertXml(expected, xmlActual);

        }

        [Test]
        public void GetTypes_Generics_ReturnsCorrectXml()
        {
            var testFileDirectory = "TestTypes/Generics/";
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

            var expected = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""1"">
    <types>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.ExtendsGenericInterface</id>
            <parentId></parentId>
            <name>ExtendsGenericInterface</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>ExtendsGenericInterface.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass`1</id>
            <parentId></parentId>
            <name>GenericClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass</id>
            <parentId></parentId>
            <name>GenericClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericStructWithConstraints`1</id>
            <parentId></parentId>
            <name>GenericStructWithConstraints</name>
            <kind>Struct</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericStructWithConstraints.cs</path>

            </relativeFilePaths>
        </type></types></doc>";

            AssertXml(expected, xmlActual);
        }

        [Test]
        public void GetType_Documentation_ReturnsCorrectXml()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/CommonTypes/"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass",  "AClass.cs", "AFolder/AClass.part2.cs");
            Console.WriteLine(actualXml);

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""AClass"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"">
        <interfaces>
            <interface typeId=""System.Collections.IEnumerable"" typeName=""IEnumerable""/>
            <interface typeId=""System.ICloneable"" typeName=""ICloneable""/>
        </interfaces>
        <xmldoc><![CDATA[
            <summary>I have a summary</summary>
            <example>In a partial type...</example>
            Here is some more docs
        ]]></xmldoc>

        <member name=""Foo"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int""/>
                <parameters>
                    <parameter name=""i"" typeId=""System.Int32"" typeName=""int""/>
                </parameters>
            </signature>
            <xmldoc><![CDATA[
                <summary>So do I</summary>
                <returns>whatever you want.</returns>
            ]]></xmldoc>                
        </member>

        <member name=""VoidProtectedMethod"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Protected</accessibility>
                <return typeId=""System.Void"" typeName=""void""/>
                <parameters>
                </parameters>
            </signature>
            <xmldoc><![CDATA[
                some docs
            ]]></xmldoc>                
        </member>
        <member name=""System.Collections.IEnumerable.GetEnumerator"" type=""Method"" methodKind=""ExplicitInterfaceImplementation"">
            <signature>
                <accessibility>Private</accessibility>
                <return typeId=""System.Collections.IEnumerator"" typeName=""System.Collections.IEnumerator""/>
                <parameters></parameters>
            </signature>
            <xmldoc><![CDATA[
                <summary>
                Explicit Implementation
                </summary>
                <returns></returns>
            ]]></xmldoc>
        </member>
        <member name=""Clone"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Object"" typeName=""object""/>
                <parameters></parameters>
            </signature>
            <xmldoc><![CDATA[    
            ]]></xmldoc>
        </member>
    </member>
</doc>";
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void GetTypes_ExcludeDirectories()
        {
            var handler = new XMLDocHandler(new CompilationParameters("TestTypes/", new []{ "TestTypes/CommonTypes" }, new string[0], new[] {typeof(object).Assembly.Location}));
            string actualXml = handler.GetTypesXml();
            Assert.That(actualXml, !Contains.Substring("AClass"));
        }

        [Test]
        public void Test_Documentation_Under_Conditional_Compilation_Symbols_Works()
        {
            Assert.Inconclusive("Not implementated yet");
        }
        
        public struct TestIsReportedData
        {
            public string typeId;
            public string sourceFile;
            public string expectedXml;
            public bool exact;
        }

        static IEnumerable<TestCaseData> TestIsReportedTestCases()
        {
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithField.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithField",
                    expectedXml = @"<member name=""value"" type=""Field"">
<signature>
    <accessibility>Public</accessibility>
    <type typeId=""System.Int32"" typeName=""int""/>
</signature>
<attributes>
    <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
</attributes>
<xmldoc><![CDATA[
    <summary>
    Value field
    </summary>
]]></xmldoc>
</member>"
                }).SetName("Field_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Generics/GenericClass.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass`1",
                    expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""GenericClass`1"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics"">
        <typeParameters>
            <typeParameter name=""T""/>
        </typeParameters>
        <xmldoc>
            <![CDATA[
    <summary>
    Existing Docs for GenericClass-T
    </summary>]]>
        </xmldoc>
        <member name=""Foo"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Void"" typeName=""void""/>
                <parameters></parameters>
            </signature>
            <xmldoc>
                <![CDATA[
    <summary>
    Existing GenericClass-T.Foo
    </summary>

]]>
            </xmldoc>
        </member>
    </member>
</doc>",
                    exact = true
                }).SetName("Generic_Types");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithProperty.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithProperty",
                    expectedXml = @"<member name=""Value"" type=""Property"">
<signature>
    <accessibility>Public</accessibility>
    <type typeId=""System.Int32"" typeName=""int""/>
    <get><accessibility>Public</accessibility></get>
    <parameters></parameters>
</signature>
<attributes>
    <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
</attributes>
<xmldoc><![CDATA[
                
    <summary>
    Value property
    </summary>


]]></xmldoc>
</member>"
                }).SetName("Property");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithIndexer.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithIndexer",
                    expectedXml = @"  <?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""ClassWithIndexer"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"">
        
        <xmldoc><![CDATA[
        
        ]]></xmldoc><member name=""this[]"" type=""Property"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Int32"" typeName=""int""/>

                <get><accessibility>Public</accessibility></get>
                <set><accessibility>Protected</accessibility></set>
                <parameters><parameter name=""a"" typeId=""System.Int32"" typeName=""int""/>
                </parameters>
            </signature>
            <xmldoc><![CDATA[
                
    <summary>
    Indexer property
    </summary>


            ]]></xmldoc>
        </member>
</member></doc>",
                    exact = true
                }).SetName("Property_With_Indexer");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/CommonTypes/AClass.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.INestedInterface",
                    expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""INestedInterface"" type=""Interface"" containingType=""AClass"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"">
        <xmldoc><![CDATA[
            <summary>
            I am a nested interface
            </summary>
        ]]></xmldoc>
    </member>
</doc>",
                    exact = true
                }).SetName("Inner_Types");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassInGlobalNamespace.cs",
                    typeId = "ClassInGlobalNamespace",
                    expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassInGlobalNamespace"" type=""Class"" namespace="""">
        <xmldoc><![CDATA[
            <summary>
            ClassInGlobalNamespace
            </summary>
        ]]></xmldoc>
    </member>
</doc>",
                    exact = true
                }).SetName("Global_Namespace");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithEvent.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithEvent",
                    expectedXml =
@"<member name=""anEvent"" type=""Event"">
    <signature>
        <accessibility>Public</accessibility>
        <type typeId=""System.Action`1"" typeName=""System.Action&lt;bool&gt;"">
            <typeArguments>
                <type typeId=""System.Boolean"" typeName=""bool""/>
            </typeArguments>
        </type>
    </signature>
    <attributes>
        <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
    </attributes>
    <xmldoc><![CDATA[
    <summary>
    anEvent
    </summary>
    ]]></xmldoc>
</member>"
                }).SetName("Event_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithEventAddRemove.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithEventAddRemove",
                    expectedXml =
                        @"<member name=""anEvent"" type=""Event"">
    <signature>
        <accessibility>Public</accessibility>
        <type typeId=""System.Action`1"" typeName=""System.Action&lt;System.Func&lt;bool&gt;&gt;"">
            <typeArguments>
                <type typeId=""System.Func`1"" typeName=""System.Func&lt;bool&gt;"">
                    <typeArguments>
                        <type typeId=""System.Boolean"" typeName=""bool""/>
                    </typeArguments>
                </type>
            </typeArguments>
        </type>
    </signature>
    <xmldoc><![CDATA[
    <summary>
    anEvent
    </summary>
    ]]></xmldoc>
</member>"
                }).SetName("Event_With_Add_Remove_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithOperator.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOperator",
                    expectedXml =
                        @"<member name=""op_Addition"" type=""Method"" methodKind=""UserDefinedOperator"" isStatic=""true"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Int32"" typeName=""int""/>
        <parameters>
            <parameter name=""classWithOperator"" typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOperator"" typeName=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOperator""/>
            <parameter name=""other"" typeId=""System.Int32"" typeName=""int""/>
        </parameters>
    </signature>
    <xmldoc><![CDATA[
        <summary>
        Plus Operator
        </summary>]]></xmldoc>
</member>"
                }).SetName("Operator_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithConstructor.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithConstructor",
                    expectedXml =
                        @"<member name="".ctor"" type=""Method"" methodKind=""Constructor"">
    <signature>
        <accessibility>Public</accessibility>
        <parameters></parameters>
    </signature>
    <xmldoc><![CDATA[<summary>A Constructor</summary>]]></xmldoc>
</member>"
                }).SetName("Constructor_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Generics/GenericStructWithConstraints.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericStructWithConstraints`1",
                    expectedXml =
                        @"<member name=""GenericStructWithConstraints`1"" type=""Struct"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics"">
        <typeParameters>
            <typeParameter name=""T"" hasConstructorConstraint=""true"" hasReferenceTypeConstraint=""true"">
                <type typeId=""System.Collections.Generic.IList`1"" typeName=""System.Collections.Generic.IList&lt;Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass&gt;"">
                    <typeArguments>
                        <type typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass"" typeName=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass""/>
                    </typeArguments>
                </type>
            </typeParameter>
        </typeParameters>
        <xmldoc>
            <![CDATA[
            <summary>
                Existing Docs for GenericStructWithConstraints-T
            </summary>
            ]]>
        </xmldoc>
        <member name=""GenericMethodWithGenericConstraint`1"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Void"" typeName=""void""/>
                <parameters></parameters>
                <typeParameters>
                    <typeParameter name=""T2"">
                        <typeParameter declaringTypeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericStructWithConstraints`1"" name=""T""/>
                        <attributes>
                            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
                        </attributes>
                    </typeParameter>
                </typeParameters>
            </signature>
            <xmldoc>
                <![CDATA[
                <summary>
                    Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint-T2
                </summary>
                ]]>
            </xmldoc>
        </member>
        <member name=""GenericMethodWithGenericConstraint"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Void"" typeName=""void""/>
                <parameters></parameters>
            </signature>
            <xmldoc>
                <![CDATA[
                <summary>
                    Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint
                </summary>
                ]]>
            </xmldoc>
        </member>
  </member>"
                }).SetName("GenericStructWithConstraints");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithExtensionMethods.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithExtensionMethods",
                    expectedXml =
                        @"<member name=""ClassWithExtensionMethods"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" isStatic=""true"">
<xmldoc>
    <![CDATA[]]>
</xmldoc>
<member name=""ExtensionMethod"" type=""Method"" methodKind=""Ordinary"" isStatic=""true"" isExtensionMethod=""true"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Int32"" typeName=""int""/>
        <parameters>
            <parameter name=""s"" typeId=""System.String"" typeName=""string""/>
        </parameters>
    </signature>
    <attributes>
        <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
    </attributes>
    <xmldoc><![CDATA[
        <summary>
        Extension method
        </summary>]]></xmldoc>
</member>
</member>"
                }).SetName("Extension_Method_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithOptionalParameters.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOptionalParameters",
                    expectedXml =
                        @"<member name=""OptionalInt"" type=""Method"" methodKind=""Ordinary"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters>
            <parameter name=""i"" typeId=""System.Int32"" typeName=""int"" isOptional=""true"" defaultValue=""3""/>
        </parameters>
    </signature>
    <xmldoc><![CDATA[<summary>
        OptionalInt
        </summary>]]>
    </xmldoc>
</member>
<member name=""OptionalNoDefaultValue"" type=""Method"" methodKind=""Ordinary"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters>
            <parameter name=""s"" typeId=""System.String"" typeName=""string"" isOptional=""true"">
                <attributes>
                    <attribute typeId=""System.Runtime.InteropServices.OptionalAttribute""/>
                </attributes>
            </parameter>
        </parameters>
    </signature>
    <xmldoc><![CDATA[<summary>
        OptionalNoDefaultValue
        </summary>]]>
    </xmldoc>
</member>
<member name=""OptionalConstValue"" type=""Method"" methodKind=""Ordinary"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters>
            <parameter name=""f"" typeId=""System.Single"" typeName=""float"" isOptional=""true"" defaultValue=""4""/>
        </parameters>
    </signature>
    <xmldoc><![CDATA[<summary>
        OptionalConstValue
        </summary>]]>
    </xmldoc>
</member>
<member name=""OptionalDefaultStruct"" type=""Method"" methodKind=""Ordinary"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters>
            <parameter name=""s"" typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOptionalParameters.AStruct"" typeName=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOptionalParameters.AStruct"" isOptional=""true"" defaultValue=""default""/>
        </parameters>
    </signature>
    <xmldoc><![CDATA[<summary>
        OptionalDefaultStruct
        </summary>]]>
    </xmldoc>
</member>"
                }).SetName("Optional_Parameter_Is_Reported");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/CommonTypes/GlobalDelegate.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.ADelegate",
                    expectedXml =
                        @"<member name=""ADelegate"" type=""Delegate"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""System.MulticastDelegate"" isSealed=""true"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Int32"" typeName=""int""/>
        <parameters>
            <parameter name=""o"" typeId=""System.Object"" typeName=""object""/>
        </parameters>
    </signature>
    <xmldoc><![CDATA[
        <summary>
        A Delegate
        </summary>]]>
    </xmldoc>
</member>"
                }).SetName("Global_Delegate");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/CommonTypes/AClass.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.Delegate",
                    expectedXml =
                        @"<member name=""Delegate"" type=""Delegate"" containingType=""AClass"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""System.MulticastDelegate"" isSealed=""true"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters></parameters>
    </signature>
    <xmldoc><![CDATA[
        <summary>
        Delegate
        </summary>]]>
    </xmldoc>
</member>"
                }).SetName("Inner_Delegate");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/DerivedClass.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.DerivedClass",
                    expectedXml =
                        @"<member name=""DerivedClass"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.SimpleClassWithXmlDoc"">
    <xmldoc><![CDATA[
        <summary>
        DerivedClass docs
        </summary>]]>
    </xmldoc>
</member>"
                }).SetName("Derived_Class");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Attributes/ClassWithAttributes.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.ClassWithAttributes",
                    expectedXml =
                        @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithAttributes"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes"">

        <attributes>
            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute"">
                <constructorArguments>
                    <argument value='50'/>
                </constructorArguments>
                <namedArguments>
                    <argument name=""AnEnum"" value='Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.AnEnum.Value'/>
                </namedArguments>
            </attribute>
        </attributes>
        <xmldoc>
            <![CDATA[]]>
        </xmldoc>
        <member name=""MethodWithParameter"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Void"" typeName=""void""/>
                <parameters>
                    <parameter name=""i"" typeId=""System.Int32"" typeName=""int"">
                        <attributes>
                            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
                        </attributes>
                    </parameter>
                </parameters>
            </signature>
            <xmldoc>
                <![CDATA[]]>
            </xmldoc>
        </member>
        <member name=""MethodWithReturn"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int"">
                    <attributes>
                        <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
                    </attributes>
                </return>
                <parameters></parameters>
            </signature>
            <xmldoc>
                <![CDATA[]]>
            </xmldoc>
        </member>
    </member>
</doc>"
                }).SetName("Class_With_Attributes");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Attributes/ClassWithAttributeWithStringArgument.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.ClassWithAttributeWithStringArgument",
                    expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithAttributeWithStringArgument"" type=""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes"">
        <attributes>
            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute"">
                <constructorArguments>
                    <argument value='""string""'/>
                </constructorArguments>
            </attribute>
        </attributes>
        <xmldoc><![CDATA[]]></xmldoc>
    </member>
</doc>",
                    exact = true
                }).SetName("Class_With_Attribute_With_String_Argument");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Attributes/StructWithAttributes.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.StructWithAttributes",
                    expectedXml =
                        @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""StructWithAttributes"" type=""Struct"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes"">
        <attributes>
            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
        </attributes>
        <xmldoc>
            <![CDATA[]]>
        </xmldoc>
    </member>
</doc>"
                }).SetName("Struct_With_Attributes");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Attributes/IInterfaceWithAttributes.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.IInterfaceWithAttributes",
                    expectedXml =
                        @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""IInterfaceWithAttributes"" type=""Interface"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes"">
        <attributes>
            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
        </attributes>
        <xmldoc>
            <![CDATA[]]>
        </xmldoc>
    </member>
</doc>"
                }).SetName("Interface_With_Attributes");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/Attributes/DelegateWithAttributes.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.DelegateWithAttributes",
                    expectedXml =
                        @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""DelegateWithAttributes"" type=""Delegate"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes"" inherits=""System.MulticastDelegate"" isSealed=""true"">
        <signature>
            <accessibility>Public
            </accessibility>
            <return typeId=""System.Void"" typeName=""void""/>
            <parameters>
            </parameters>
        </signature>
        <attributes>
            <attribute typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes.TestPublicAttribute""/>
        </attributes>
        <xmldoc>
            <![CDATA[]]>
        </xmldoc>
    </member>
</doc>"
                }).SetName("Delegate_With_Attributes");
            yield return new TestCaseData(
                new TestIsReportedData
                {
                    sourceFile = "TestTypes/ClassWithParams.cs",
                    typeId = "Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithParams",
                    expectedXml =
                        @"
<member name=""Method"" type=""Method"" methodKind=""Ordinary"">
    <signature>
        <accessibility>Public</accessibility>
        <return typeId=""System.Void"" typeName=""void""/>
        <parameters>
            <parameter name=""ints"" typeId=""System.Int32[]"" typeName=""int[]"" isParams=""true""/>
        </parameters>
    </signature>
    <xmldoc>
        <![CDATA[
    <summary>
    Params field
    </summary>]]>
    </xmldoc>
</member>"
                }).SetName("Params_Parameter");
        }

        [Test]
        [TestCaseSource(nameof(TestIsReportedTestCases))]
        public void Test_Is_Reported(TestIsReportedData data)
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("."));
            string actualXml = handler.GetTypeDocumentation(data.typeId, data.sourceFile);
            Console.WriteLine(actualXml);

            AssertValidXml(actualXml);
            if (data.exact)
                AssertXml(data.expectedXml, actualXml);
            else
                AssertXmlContains(data.expectedXml, actualXml);
        }

        private void AssertValidXml(string actualXml)
        {
            var doc = new XmlDocument();
            Assert.DoesNotThrow(() => doc.LoadXml(actualXml), $@"Xml parse error. Xml:
{actualXml}");
        }
    }
}
