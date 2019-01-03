using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Unity.DocTool.XMLDocHandler.FindMissingDocs;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class TestFindMissingDocs
    {
        [Test]
        public void FindMissingDocsOnSingleFileReturnsMembersMissingDocs()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "FindMissingDocsOnSingleFileReturnsMembersMissingDocs");
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);

            Directory.CreateDirectory(tempPath);
            File.WriteAllText(Path.Combine(tempPath, "testfile.cs"),
                @"public class TypeMissingDocs
{
    ///<summary> I have docs </summary>
    public void MethodWithDocs() {}
    public void MethodWithoutDocs() {}
}");
            var foundNames = new List<string>();
            Program.FindMissingDocs(tempPath, null, null, name => foundNames.Add(name));

            Assert.That(foundNames, Is.EquivalentTo(new[] {"TypeMissingDocs", "TypeMissingDocs.MethodWithoutDocs"}));
        }
    }
}
