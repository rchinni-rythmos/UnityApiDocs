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
        }

        //TODO: Add tests for: Fields, Methods, Events, Operators, Ctors, Static / Instance / Generics, Extension methods
        //TODO: Add tests for: Partials, Formating, Ensure that we are not deleting non xmldoc
        //TODO: Add tests for: Enuns, Interfaces, Delegates, Structs
        //TODO: Add tests for: Inner Types, Namespaces, generics of same name
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
