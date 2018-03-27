using System.IO;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class SaveXmlTests : XmlDocHandlerTestBase
    {
        [Test]
        public void SavesBackToSource()
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
