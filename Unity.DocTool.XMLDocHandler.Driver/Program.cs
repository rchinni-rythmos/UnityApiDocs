using System.IO;

namespace Unity.DocTool.XMLDocHandler.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new XMLDocHandler(new CompilationParameters(args[0], new string[0], new string[0], new [] {typeof(object).Assembly.Location}));
            var typesXml = handler.GetTypesXml();
            File.WriteAllText(args[1], typesXml);
        }
    }
}
