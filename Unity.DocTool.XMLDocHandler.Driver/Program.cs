using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Unity.Options;

namespace Unity.DocTool.XMLDocHandler.Driver
{
    class Program
    {
        [ProgramOptions]
        internal class DriverOptions
        {
            public static string RootPath;
            public static string[] ExcludedPaths;
            public static string[] Defines;
            public static string OutputDirectory;
        }

        private static string TestFileDirectory = "TestFiles";

        static void Main(string[] args)
        {
            OptionsParser.Prepare(args, typeof(Program).Assembly);
            var handler = new XMLDocHandler(new CompilationParameters(
                DriverOptions.RootPath ?? ".",
                DriverOptions.ExcludedPaths ?? new string[0], 
                DriverOptions.Defines ?? new string[0], 
                new [] {typeof(object).Assembly.Location}));

            string typesXml = handler.GetTypesXml();
            var isOutputDirectorySpecified = !string.IsNullOrEmpty(DriverOptions.OutputDirectory);
            if (isOutputDirectorySpecified)
                File.WriteAllText(args[1], Path.Combine(DriverOptions.OutputDirectory, "GetTypes.xml"));

            XmlDocument getTypesXml = new XmlDocument();

            getTypesXml.LoadXml(typesXml);

            foreach (XmlNode typeNode in getTypesXml.SelectNodes("//type"))
            {
                var id = typeNode.SelectSingleNode("id").InnerText;
                var pathNodes = typeNode.SelectNodes("relativeFilePaths/path");
                var paths = new List<string>();
                foreach (XmlNode pathNode in pathNodes)
                    paths.Add(pathNode.InnerText);

                var random = new Random();
                string typeXml = handler.GetTypeDocumentation(id, paths.ToArray());
                XmlDocument getTypeXml = new XmlDocument();
                getTypeXml.LoadXml(typeXml);

                if (isOutputDirectorySpecified)
                    File.WriteAllText(Path.Combine(DriverOptions.OutputDirectory, FixupFilename(id)), typeXml);

                var xmlDocNodes = getTypeXml.SelectNodes("//xmldoc");
                HashSet<string> randomComments = new HashSet<string>();
                foreach (XmlNode xmlDocNode in xmlDocNodes)
                {
                    var randomComment = random.Next().ToString();
                    randomComments.Add(randomComment);
                    xmlDocNode.ReplaceChild(getTypeXml.CreateCDataSection(randomComment), xmlDocNode.FirstChild);
                }

                var tempPaths = paths.Select(p =>
                {
                    var tempPath = Path.GetTempFileName();
                    File.Copy(p, tempPath, true);
                    return tempPath;
                }).ToArray();

                var getTypeXmlString = getTypeXml.OuterXml;
                handler.SetType(getTypeXmlString, tempPaths);
                foreach (var path in tempPaths)
                {
                    var content = File.ReadAllText(path);
                    randomComments.RemoveWhere(comment => content.Contains("/// " + comment));
                }

                if (randomComments.Count > 0)
                    Console.WriteLine("Did not write all comments back properly. Xml used to set:\n" + getTypeXmlString);
            }
        }

        private static string FixupFilename(string id)
        {
            foreach (var character in Path.GetInvalidFileNameChars())
                id = id.Replace(character, ' ');
            return id;
        }
    }
}
