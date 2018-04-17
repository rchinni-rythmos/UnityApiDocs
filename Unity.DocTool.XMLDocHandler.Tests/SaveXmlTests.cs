using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class SaveXmlTests : XmlDocHandlerTestBase
    {
        public struct UpdateTestData
        {
            public string newDocXml;
            public string expectedSource;
            public string sourcePath;
        }

        public static IEnumerable<TestCaseData> UpdateTestCases()
        {
            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""ClassWithProperty"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
            <member name = ""Value"" type=""Property"">
                <signature>
<accessibility>Public</accessibility>
<type typeId=""System.Int32"" typeName=""int"" />

<get><accessibility>Public</accessibility></get>
<parameters></parameters></signature>
            <xmldoc>
    <![CDATA[<summary>
    New Value Propery
    </summary>
]]>
            </xmldoc>
        </member>
</member></doc>",
                    expectedSource = @"
namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{    
    class ClassWithProperty
    {
    /// <summary>
    ///New Value Propery
    ///</summary>
        public int Value
",
                    sourcePath = "TestTypes/ClassWithProperty.cs"
                }).SetName("Update_Property");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf -8"" standalone =""yes"" ?>
    <doc version=""3"">
        <member name=""SimpleClassWithXmlDoc"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
<![CDATA[
<summary>Updated Doc</summary>
]]>
        </xmldoc></member></doc>
",
                    expectedSource = @"
    /// <summary>Updated Doc</summary>
    public class SimpleClassWithXmlDoc
    {
        /// <summary>
        /// Foo XmlDoc
        /// </summary>
        public void Foo() {}
}",
                    sourcePath = "TestTypes/SimpleClassWithXmlDoc.cs"
                }).SetName("Update_Class");
            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf -8"" standalone =""yes"" ?>
    <doc version=""3"">
        <member name=""AnEnum"" type = ""Enum"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Enum"">
        
        <xmldoc>
<![CDATA[
<summary>
Some Docs
</summary>
]]>
        </xmldoc></member></doc>
",
                    expectedSource = @"
    /// <summary>
    /// Some Docs
    /// </summary>
    public enum AnEnum
    {
    }",
                    sourcePath = "TestTypes/CommonTypes/AnEnum.cs"
                }).SetName("Add_To_Enum");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""INestedInterface"" type = ""Interface"" containingType=""AClass"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits="""">
        <xmldoc>
            <![CDATA[<summary>
Updated Docs
</summary>]]>
        </xmldoc>
    </member>
</doc>",
                    expectedSource = @"    /// <summary>
    /// I have a summary
    /// </summary>
    public partial class AClass : IEnumerable, ICloneable
    {
        /// <summary>
        /// Updated Docs
        /// </summary>
        public interface INestedInterface
        {",
                    sourcePath = "TestTypes/CommonTypes/AClass.cs"
                }).SetName("Update_Nested_Interface");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""GenericClass`1"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics"" inherits=""Object"">
        <typeParameters>
            <typeParameter name=""T""/>
        </typeParameters>
        <xmldoc>
            <![CDATA[<summary>New Class docs</summary>]]>
        </xmldoc>
        <member name = ""Foo"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Void"" typeName=""void"" />
                <parameters></parameters>
            </signature>
            <xmldoc>
                <![CDATA[<summary>New Foo docs</summary>]]>
            </xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
    /// <summary>New Class docs</summary>
    public class GenericClass<T>
    {
        /// <summary>New Foo docs</summary>
        public void Foo()
        {}
    }
    /// <summary>
    /// Existing Docs for GenericClass
    /// </summary>
    public class GenericClass
    {
        /// <summary>
        /// Existing Docs for GenericClass.Foo
        /// </summary>
        public void Foo()
        {}
    }",
                    sourcePath = "TestTypes/Generics/GenericClass.cs"
                }).SetName("Update_Generic");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""INestedInterface"" type = ""Interface"" containingType=""AClass"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits="""">
        <xmldoc>
            <![CDATA[<summary>
Updated Docs
</summary>]]>
        </xmldoc>
    </member>
</doc>",
                    expectedSource = @"    /// <summary>
    /// I have a summary
    /// </summary>
    public partial class AClass : IEnumerable, ICloneable
    {
        /// <summary>
        /// Updated Docs
        /// </summary>
        public interface INestedInterface
        {",
                    sourcePath = "TestTypes/CommonTypes/AClass.cs"
                }).SetName("Update_Nested_Interface");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""ClassWithXmlDocsAndNormalComments"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>Only the summary</summary>]]>
        </xmldoc>
</member></doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    // Type-Prefix
    /// <summary>Only the summary</summary>
    // Type-Sufix
    public class ClassWithXmlDocsAndNormalComments
    {",
                    sourcePath = "TestTypes/ClassWithXmlDocsAndNormalComments.cs"
                }).SetName("Update_With_Non_Xml_Docs");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""ClassWithMultipleXmlDocs"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>Only the summary</summary>]]>
        </xmldoc>
</member></doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>Only the summary</summary> 
    // after summary
    // after remarks
    public class ClassWithMultipleXmlDocs
    {",
                    sourcePath = "TestTypes/ClassWithMultipleXmlDocs.cs"
                }).SetName("Update_Multiple_Xml_Docs");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithField"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <member name = ""value"" type=""Field"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Int32"" typeName=""int"" />
            </signature>
            <xmldoc>
                <![CDATA[<summary>New Docs</summary>]]>
            </xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithField
    {
        /// <summary>New Docs</summary>
        public int value;
    }
}",
                    sourcePath = "TestTypes/ClassWithField.cs"
                }).SetName("Update_Field");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithMultipleFieldsOnDeclaration"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <member name = ""value2"" type=""Field"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Object"" typeName=""System.Object"" />
            </signature>
            <xmldoc>
                <![CDATA[<summary>New Docs</summary>]]>
            </xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithMultipleFieldsOnDeclaration
    {
        /// <summary>
        /// Value field 1
        /// </summary>
        public System.Object value1,

            /// <summary>New Docs</summary>
            value2;
    }
}
",
                    sourcePath = "TestTypes/ClassWithMultipleFieldsOnDeclaration.cs"
                }).SetName("Update_Second_Field_On_Declaration");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithField"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New ClassWithField Docs</summary>]]>
        </xmldoc>
        <member name = ""value"" type=""Field"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Int32"" typeName=""int"" />
            </signature>
            <xmldoc>
                <![CDATA[<summary>New value Docs</summary>]]>
            </xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>New ClassWithField Docs</summary>
    class ClassWithField
    {
        /// <summary>New value Docs</summary>
        public int value;
    }
}",
                    sourcePath = "TestTypes/ClassWithField.cs"
                }).SetName("Update_Field_And_Enclosing_Class");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""AClass"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        
        <member name = ""Foo"" type=""Method"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int"" />
                <parameters>
                    <parameter name=""i"" typeId=""System.Int32"" typeName=""int"" />
                </parameters>
            </signature>
            <xmldoc><![CDATA[<summary>New method docs</summary>]]></xmldoc>                
        </member>
    </member>
</doc>",
                    expectedSource = @"
    /// <summary>New class docs</summary>
    public partial class AClass : IEnumerable, ICloneable
    {
        /// <summary>
        /// I am a nested interface
        /// </summary>
        public interface INestedInterface
        {
            
        }

        private class APrivateClass
        {
        }

        /// <summary>New method docs</summary>
        public int Foo(int i)
        {",
                    sourcePath = "TestTypes/CommonTypes/AClass.cs"
                }).SetName("Update_Method");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithEvent"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        <member name = ""anEvent"" type=""Event"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Action`1"" typeName=""System.Action&lt;bool&gt;"">
                    <typeArguments>
                        <type typeId=""System.Boolean"" typeName=""bool"" />
                    </typeArguments>
                </type>
            </signature>
            <xmldoc><![CDATA[<summary>new docs</summary>]]></xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>New class docs</summary>
    class ClassWithEvent
    {
        /// <summary>new docs</summary>
        public event System.Action<bool> anEvent;
    }
}
",
                    sourcePath = "TestTypes/ClassWithEvent.cs"
                }).SetName("Update_Event");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithEventAddRemove"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        <member name = ""anEvent"" type=""Event"">
            <signature>
                <accessibility>Public</accessibility>
                <type typeId=""System.Action`1"" typeName=""System.Action&lt;System.Func&lt;bool&gt;&gt;"">
                    <typeArguments>
                        <type typeId=""System.Func`1"" typeName=""System.Func&lt;bool&gt;"">
                            <typeArguments>
                                <type typeId=""System.Boolean"" typeName=""bool"" />
                            </typeArguments>
                        </type>
                    </typeArguments>
                </type>
            </signature>
            <xmldoc><![CDATA[<summary>new docs</summary>]]></xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>New class docs</summary>
    class ClassWithEventAddRemove
    {
        /// <summary>new docs</summary>
        public event System.Action<System.Func<bool>> anEvent
        {
            add { }
            remove { }
        }
    }
}",
                    sourcePath = "TestTypes/ClassWithEventAddRemove.cs"
                }).SetName("Update_Event_With_Add_Remove");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithOperator"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        <member name = ""op_Addition"" type=""Method"" methodKind=""UserDefinedOperator"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int"" />
                <parameters>
                    <parameter name=""classWithOperator"" typeId=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOperator"" typeName=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithOperator"" />
                    <parameter name=""other"" typeId=""System.Int32"" typeName=""int"" />
                </parameters>
            </signature>
            <xmldoc><![CDATA[<summary>new docs</summary>]]></xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>New class docs</summary>
    class ClassWithOperator
    {
        /// <summary>new docs</summary>
        public static int operator +(ClassWithOperator classWithOperator, int other)
        {
            return 1;
        }
    }
}",
                    sourcePath = "TestTypes/ClassWithOperator.cs"
                }).SetName("Update_Operator");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithConstructor"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        <member name = "".ctor"" type=""Method"" methodKind=""Constructor"">
            <signature>
                <accessibility>Public</accessibility>
            </signature>
            <xmldoc><![CDATA[<summary>new docs</summary>]]></xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>New class docs</summary>
    class ClassWithConstructor
    {
        /// <summary>new docs</summary>
        public ClassWithConstructor() { }
    }
}",
                    sourcePath = "TestTypes/ClassWithConstructor.cs"
                }).SetName("Update_Constructor");

            yield return new TestCaseData(
                new UpdateTestData
                {
                    newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""ClassWithExtensionMethods"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        <xmldoc>
            <![CDATA[<summary>New class docs</summary>]]>
        </xmldoc>
        <member name = ""ExtensionMethod"" type=""Method"" methodKind=""Ordinary"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int"" />
                <parameters>
                    <parameter name=""s"" typeId=""System.String"" typeName=""string"" isThis=""true""/>
                </parameters>
            </signature>
            <xmldoc><![CDATA[<summary>new docs</summary>]]></xmldoc>
        </member>
    </member>
</doc>",
                    expectedSource = @"
    /// <summary>New class docs</summary>
    static class ClassWithExtensionMethods
    {
        /// <summary>new docs</summary>
        public static int ExtensionMethod(this string s)
        {",
                    sourcePath = "TestTypes/ClassWithExtensionMethods.cs"
                }).SetName("Update_Extension_Method");
        }

        //TODO: Add tests for: Extension methods
        //TODO: Add tests for: Formating
        //TODO: Add tests for: Delegates
        [Test]
        [TestCaseSource(nameof(UpdateTestCases))]
        public void Update(UpdateTestData data)
        {
            var testFilePath = Path.GetTempFileName();

            File.Copy(data.sourcePath, testFilePath, true);
            var handler = new XMLDocHandler(MakeCompilationParameters(Path.GetDirectoryName(testFilePath)));

            handler.SetType(data.newDocXml, Path.GetFileName(testFilePath));

            var actualSource = File.ReadAllText(testFilePath);
            AssertSourceContains(data.expectedSource, actualSource);
        }

        // Partials: 
        //          1) Udating comment on single partial, 
        //          2) Updating comments in multiple partials (we should remove the comment in all but one partial)
        //          3) Adding comments in multiple partials (ensure that comment end up in only one of the partials)

        public struct UpdatePartialsTestData
        {
            public string filename1, filename2;
            public string newContent;
            public string expectedFile1Source;
            public string expectedFile2Source;
        }

        public static IEnumerable<TestCaseData> UpdatePartialsTestCases()
        {
            yield return new TestCaseData(
                new UpdatePartialsTestData
                {
                    filename1 = "TestTypes/CommonTypes/AClass.cs",
                    filename2 = "TestTypes/CommonTypes/AFolder/AClass.part2.cs",
                    newContent = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""AClass"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Object"">
        <xmldoc><![CDATA[<summary>new doc</summary>]]></xmldoc>
    </member>
</doc>
",
                    expectedFile1Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes
{
    public partial class AClass { }

    /// <summary>new doc</summary>
    public partial class AClass : IEnumerable, ICloneable
    {
",
                    expectedFile2Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes
{
    //Here is a partial for implementation details...
    public partial class AClass
    {
"
                }).SetName("Update_Partial_Class_With_Existing_Doc");
            yield return new TestCaseData(
                new UpdatePartialsTestData
                {
                    filename1 = "TestTypes/PartialInterfaceNoDocs.cs",
                    filename2 = "TestTypes/PartialInterfaceNoDocs.part2.cs",
                    newContent = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""PartialInterfaceNoDocs"" type = ""Interface"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"">
        <xmldoc><![CDATA[<summary>new doc</summary>]]></xmldoc>
    </member>
</doc>
",
                    expectedFile1Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    /// <summary>new doc</summary>
    partial interface PartialInterfaceNoDocs
    {
    }
}
",
                    expectedFile2Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    partial interface PartialInterfaceNoDocs
    {
    }
}
"
                }).SetName("Update_Partial_Interface_With_No_Existing_Doc");
            yield return new TestCaseData(
                new UpdatePartialsTestData
                {
                    filename1 = "TestTypes/PartialStructWithDocs.cs",
                    filename2 = "TestTypes/PartialStructWithDocs.part2.cs",
                    newContent = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""PartialStructWithDocs"" type = ""Struct"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"">
        <xmldoc><![CDATA[<summary>new doc</summary>]]></xmldoc>
    </member>
</doc>
",
                    expectedFile1Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    partial struct PartialStructWithDocs
    {
    }
    /// <summary>new doc</summary>
    partial struct PartialStructWithDocs
    {
    }
}
",
                    expectedFile2Source = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    partial struct PartialStructWithDocs
    {
    }
}
"
                }).SetName("Update_Partial_Struct_With_Multiple_Existing_Doc");
        }

        [Test]
        [TestCaseSource(nameof(UpdatePartialsTestCases))]
        public void Test_Update_Partials(UpdatePartialsTestData testData)
        {
            var testFilePath1 = Path.GetTempFileName();
            var testFilePath2 = Path.GetTempFileName();

            File.Copy(testData.filename1, testFilePath1, true);
            File.Copy(testData.filename2, testFilePath2, true);

            var handler = new XMLDocHandler(MakeCompilationParameters(Path.GetDirectoryName(testFilePath1)));

            handler.SetType(testData.newContent, Path.GetFileName(testFilePath1), Path.GetFileName(testFilePath2));

            var actualSource1 = File.ReadAllText(testFilePath1);
            AssertSourceContains(testData.expectedFile1Source, actualSource1);

            var actualSource2 = File.ReadAllText(testFilePath2);
            AssertSourceContains(testData.expectedFile2Source, actualSource2);
        }

        private void AssertSourceContains(string expectedSource, string actualSource, bool normalize = true)
        {
            if (normalize)
            {
                actualSource = Normalize(actualSource);
                expectedSource = Normalize(expectedSource);
            }

            Assert.IsTrue(actualSource.Contains(expectedSource), $"Expected\n {expectedSource}\nbut got\n {actualSource}");
        }
    }
}
