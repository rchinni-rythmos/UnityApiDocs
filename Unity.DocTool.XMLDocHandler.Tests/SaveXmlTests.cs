﻿using System.Collections.Generic;
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
        <xmldoc>
            <![CDATA[<summary>
Overidden Docs
</summary>]]>
        </xmldoc>
</member></doc>",
                    expectedSource = @"namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
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
    }
    /// <summary>
    /// Overidden Docs
    /// </summary>
    public class GenericClass<T>
    {
        /// <summary>
        /// Existing GenericClass-T.Foo
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
                }).SetName("Update_Type_With_Non_Xml_Docs");
        }

        //TODO: Add tests for: Fields, Methods, Events, Operators, Ctors, Static / Instance / Generics, Extension methods
        //TODO: Add tests for: Partials, Formating
        //TODO: Add tests for: Delegates, Structs
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
