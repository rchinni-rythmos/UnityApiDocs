using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.DocTool.XMLDocHandler.Extensions;

namespace Unity.DocTool.XMLDocHandler
{
    public class CompilationParameters
    {
        public IEnumerable<string> DefinedSymbols { get; }
        public string RootPath { get;  }
        public IEnumerable<string> ReferencedAssemblyPaths { get; }

        public CompilationParameters(string rootPath, IEnumerable<string> definedSymbols, IEnumerable<string> referencedAssemblyPaths)
        {
            DefinedSymbols = definedSymbols ?? throw new ArgumentNullException(nameof(definedSymbols));
            RootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
            ReferencedAssemblyPaths = referencedAssemblyPaths ?? throw new ArgumentNullException(nameof(referencedAssemblyPaths));
        }
    }

    public class XMLDocHandler
    {
        private CompilationParameters compilationParameters;

        public XMLDocHandler(CompilationParameters compilationParameters)
        {
            this.compilationParameters = compilationParameters;
        }

        public string GetTypesXml()
        {
            if (!Directory.Exists(compilationParameters.RootPath))
                throw new ArgumentException($"Directory \"{compilationParameters.RootPath}\" does not exist.");

            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse, SourceCodeKind.Regular, compilationParameters.DefinedSymbols);
            var filePaths = Directory.GetFiles(compilationParameters.RootPath, "*.cs", SearchOption.AllDirectories);
            var startIndex = compilationParameters.RootPath.Length + (compilationParameters.RootPath.EndsWith("\\") || compilationParameters.RootPath.EndsWith("/") ? 0 : 1);
            var syntaxTrees = filePaths.Select(
                p =>
                {
                    return SyntaxFactory.ParseSyntaxTree(File.ReadAllText(p), parserOptions, p.Substring(startIndex));
                }).ToArray();

            var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            compilerOptions = compilerOptions.WithAllowUnsafe(true);
            var compilation = CSharpCompilation.Create("Test", syntaxTrees, GetMetadataReferences(), compilerOptions);

            var getTypesVisitor = new GetTypesVisitor();
            foreach (var syntaxTree in syntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                getTypesVisitor.Visit(syntaxTree.GetRoot(), semanticModel);
            }

            return getTypesVisitor.GetXml();
        }
        
        public string GetTypeDocumentation(string id, params string[] paths)
        {
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse, SourceCodeKind.Regular, compilationParameters.DefinedSymbols);

            var syntaxTrees = paths.Select(p => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(Path.Combine(compilationParameters.RootPath, p)), parserOptions, p));

            var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            compilerOptions = compilerOptions.WithAllowUnsafe(true);
            var compilation = CSharpCompilation.Create("Test", syntaxTrees, GetMetadataReferences(), compilerOptions);

            var extraMemberRegEx = new Regex("\\<member name=[^\\>]+\\>|\\</member\\>", RegexOptions.Compiled);

            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);

                var descendants = syntaxTree.GetRoot().DescendantNodes();
                var res = descendants.OfType<BaseTypeDeclarationSyntax>().ToArray();
                foreach (var typeDeclaration in res)
                {
                    var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                    if (id == typeSymbol.QualifiedName(true, true))
                    {
                        var containingType = typeSymbol.ContainingType != null ? 
                            $@"containingType=""{typeSymbol.ContainingType.FullyQualifiedName(false, true)}"" " : 
                            string.Empty;
                        
                        var xml = new StringBuilder($@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""{typeSymbol.MetadataName}"" type = ""{typeSymbol.TypeKind}"" {containingType}namespace=""{typeSymbol.ContainingNamespace}"" inherits=""{BaseType(typeSymbol)}"">
        {InterfaceList(typeSymbol)}
        <xmldoc>
            <![CDATA[{ extraMemberRegEx.Replace(typeSymbol.GetDocumentationCommentXml(), "")}]]>
        </xmldoc>");

                        var members = typeSymbol.GetMembers()
                            .Where(m => m.Kind != SymbolKind.NamedType &&
                                        m.MayHaveXmlDoc() &&
                                        !m.IsImplicitlyDeclared);

                        foreach (var member in members)
                        {
                            xml.Append($@"<member name = ""{member.Name}"" type=""{member.Kind}"">
            <signature>{SignatureFor(member)}</signature>
            <xmldoc>
                <![CDATA[{ extraMemberRegEx.Replace(member.GetDocumentationCommentXml(), "")}]]>
            </xmldoc>
        </member>
");
                        }

                        xml.Append(@"</member></doc>");

                        return xml.ToString();
                    }
                }

                //getTypesVisitor.Visit(syntaxTree.GetRoot(), semanticModel);
            }

            //return getTypesVisitor.GetXml();
            throw new Exception($"Type not found Id={id}");
        }

        private IEnumerable<PortableExecutableReference> GetMetadataReferences()
        {
            return compilationParameters.ReferencedAssemblyPaths.Select(p => MetadataReference.CreateFromFile(p));
        }

        private static string BaseType(INamedTypeSymbol typeSymbol)
        {
            if (typeSymbol.TypeKind == TypeKind.Interface)
                return null;

            return typeSymbol.BaseType.TypeKind == TypeKind.Interface ? "Object" : typeSymbol.BaseType.Name;
        }

        private string InterfaceList(INamedTypeSymbol typeSymbol)
        {
            List<INamedTypeSymbol> interfaces = new List<INamedTypeSymbol>(typeSymbol.Interfaces);
            if (typeSymbol.BaseType?.TypeKind == TypeKind.Interface)
                interfaces.Add(typeSymbol.BaseType);

            if (interfaces.Count == 0)
                return String.Empty;

            return $@"<interfaces>
{String.Join(Environment.NewLine, interfaces.Select(i => $@"<interface typeId=""{i.Id()}"" typeName=""{i.Name}"" />"))}
</interfaces>";
        }

        private string SignatureFor(ISymbol member)
        {
            switch (member.Kind)
            {
                case SymbolKind.Field: return member.Name;
                case SymbolKind.Method:
                    {
                        var method = (IMethodSymbol)member;
                        var returnXml = method.Name == ".ctor" ? "" : $"<return typeId=\"{method.ReturnType.Id()}\" typeName=\"{method.ReturnType.ToDisplayString()}\" />";
                        return $@"
<accessibility>{member.DeclaredAccessibility}</accessibility>
{returnXml}
<parameters>{ParametersSignature(method.Parameters)}</parameters>";
                    }

                case SymbolKind.Property:
                    {
                        var property = (IPropertySymbol)member;
                        var returnXml = $"<type typeId=\"{property.Type.Id()}\" typeName=\"{property.Type.ToDisplayString()}\" />";
                        var accessorsXml = string.Empty;

                        if (property.GetMethod != null && property.GetMethod.IsPublicApi())
                        {
                            accessorsXml = $"\n<get><accessibility>{property.GetMethod.DeclaredAccessibility}</accessibility></get>";
                        }

                        if (property.SetMethod != null && property.SetMethod.IsPublicApi())
                        {
                            accessorsXml += $"\n<set><accessibility>{property.SetMethod.DeclaredAccessibility}</accessibility></set>";
                        }


                        return $@"
<accessibility>{member.DeclaredAccessibility}</accessibility>
{returnXml}
{accessorsXml}
<parameters>{ParametersSignature(property.Parameters)}</parameters>";
                    }

                default:
                    throw new NotImplementedException($"Unsupported type {member.Kind} : {member.Name}");
            }
        }

        private string ParametersSignature(IEnumerable<IParameterSymbol> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"<parameter name=\"{parameter.Name}\" typeId=\"{parameter.Type.Id()}\" typeName=\"{parameter.Type.ToDisplayString()}\" />");
            }
            return sb.ToString();
        }

        public void SetType(string docXml, params string[] sourcePaths)
        {
            IEnumerable<string> defines = new string[0];
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse, SourceCodeKind.Regular, defines);

            var syntaxTrees = sourcePaths.Select(path =>
            {
                var filePath = Path.Combine(compilationParameters.RootPath, path);
                return SyntaxFactory.ParseSyntaxTree(
                        File.ReadAllText(filePath), parserOptions,
                        filePath);
            }).ToArray();

            var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            compilerOptions = compilerOptions.WithAllowUnsafe(true);
            var compilation = CSharpCompilation.Create("Test", syntaxTrees, GetMetadataReferences(), compilerOptions);

            var docUpdater = new XmlDocReplacerVisitor(docXml);

            foreach (var syntaxTree in syntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var result = docUpdater.Visit(syntaxTree.GetRoot(), semanticModel);

                if (result != syntaxTree.GetRoot())
                {
                    File.WriteAllText(syntaxTree.FilePath, result.ToFullString());
                }
            }
        }
    }

    internal class XMLDocExtractVisitor : CSharpSyntaxWalker
    {
        private readonly string _id;
        private SemanticModel _semanticModel;

        public XMLDocExtractVisitor(string id) : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _id = id;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);
            var symbolId = symbol.QualifiedName(true, true);
            if (symbolId == _id)
            {

                return;
            }

            base.VisitClassDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            base.VisitStructDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            base.VisitEnumDeclaration(node);
        }

        internal void Visit(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
        }

        //public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        //{
        //    base.VisitDocumentationCommentTrivia(node);
        //    var documentationTarget = node.ParentTrivia.Token.Parent;

        //    Console.WriteLine($"|{node.Content}|");
        //}

        //public override void VisitXmlElement(XmlElementSyntax node)
        //{
        //    base.VisitXmlElement(node);
        //    Console.WriteLine($"----\r\n{node}");
        //}
    }
}