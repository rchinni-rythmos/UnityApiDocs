using System.IO;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class SaveXmlTests : XmlDocHandlerTestBase
    {
        //TODO: Add tests for: Fields, Methods, Events, Operators, Ctors, Static / Instance / Generics, Extension methods
        //TODO: Add tests for: Partials, Formating, Ensure that we are not deleting non xmldoc
        //TODO: Add tests for: Enuns, Interfaces, Delegates, Structs

        [Test]
        public void Add_New_Comment_Works()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/CommonTypes/"));

            string newDocXml = @"<?xml version=""1.0"" encoding=""utf -8"" standalone =""yes"" ?>
    <doc version=""3"">
        <member name=""AnEnum"" type = ""Enum"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Enum"">
        
        <xmldoc>
<![CDATA[
<summary>
Some Docs
</summary>
]]>
        </xmldoc></member></doc>
";
            handler.SetType(newDocXml, "AnEnum.cs");
            var actualSource = File.ReadAllText("TestTypes/CommonTypes/AnEnum.cs");
            var expectedSource = @"
    /// <summary>
    /// Some Docs
    /// </summary>
    public enum AnEnum
    {
    }";
            AssertSourceContains(expectedSource, actualSource);
        }


        [Test]
        public void Update_Comments_On_Class_Works()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/"));

            string newDocXml = @"<?xml version=""1.0"" encoding=""utf -8"" standalone =""yes"" ?>
    <doc version=""3"">
        <member name=""SimpleClassWithXmlDoc"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Object"">
        <xmldoc>
<![CDATA[
<summary>Updated Doc</summary>
]]>
        </xmldoc></member></doc>
";
            handler.SetType(newDocXml, "SimpleClassWithXmlDoc.cs");

            var actualSource = File.ReadAllText("TestTypes/SimpleClassWithXmlDoc.cs");
            var expectedSource = @"
    /// <summary>Updated Doc</summary>
    public class SimpleClassWithXmlDoc
    {
        /// <summary>
        /// Foo XmlDoc
        /// </summary>
        public void Foo() {}
}";
            AssertSourceContains(expectedSource, actualSource);
        }

        [Test]
        public void Update_Comments_On_Property_Works()
        {
            var testFilePath = Path.GetTempFileName();
            File.Copy("TestTypes/ClassWithProperty.cs", testFilePath, true);
            var handler = new XMLDocHandler(MakeCompilationParameters(Path.GetDirectoryName(testFilePath)));

            string newDocXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
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
</member></doc>";

            handler.SetType(newDocXml, Path.GetFileName(testFilePath));

            var actualSource = File.ReadAllText(testFilePath);
            var expectedSource = @"
namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{    
    class ClassWithProperty
    {
    /// <summary>
    ///New Value Propery
    ///</summary>
        public int Value
";
            AssertSourceContains(expectedSource, actualSource);
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
