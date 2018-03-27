using System.IO;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class SaveXmlTests : XmlDocHandlerTestBase
    {
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
        public void Update_Comments_Works()
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
