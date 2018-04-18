using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
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

                        var attributes = typeSymbol.IsValueType ? "" : $@" inherits=""{BaseType(typeSymbol)}""";
                        if (typeSymbol.IsStatic)
                            attributes += @" isStatic=""true""";
                        if (typeSymbol.IsSealed && !typeSymbol.IsValueType)
                            attributes += @" isSealed=""true""";

                        var xml = new StringBuilder($@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""{typeSymbol.MetadataName}"" type=""{typeSymbol.TypeKind}"" {containingType}namespace=""{typeSymbol.ContainingNamespace}""{attributes}>
        {InterfaceList(typeSymbol)}
        {TypeParametersXmlForDeclaration(typeSymbol.TypeParameters)}
        <xmldoc>
            <![CDATA[{ extraMemberRegEx.Replace(typeSymbol.GetDocumentationCommentXml(), "")}]]>
        </xmldoc>");

                        var members = typeSymbol.GetMembers()
                            .Where(m => m.Kind != SymbolKind.NamedType &&
                                        m.MayHaveXmlDoc() &&
                                        !m.IsImplicitlyDeclared);

                        foreach (var member in members)
                        {
                            string methodAttributes = "";
                            var memberName = member.Name;
                            int typeParameterCount = 0;
                            if (member.Kind == SymbolKind.Method)
                            {
                                var methodSymbol = (IMethodSymbol)member;
                                if (methodSymbol.TypeParameters.Length > 0)
                                    memberName += "`" + methodSymbol.TypeParameters.Length;

                                methodAttributes = $@" methodKind=""{methodSymbol.MethodKind}""";
                                if (methodSymbol.IsStatic)
                                    methodAttributes += @" isStatic=""true""";
                                if (methodSymbol.IsExtensionMethod)
                                    methodAttributes += @" isExtensionMethod=""true""";
                            }

                            xml.Append($@"<member name = ""{memberName}"" type=""{member.Kind}""{methodAttributes}>
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
            }

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
{String.Join(Environment.NewLine, interfaces.Select(i => $@"<interface typeId=""{i.Id()}"" typeName=""{i.Name}""/>"))}
</interfaces>";
        }

        private string SignatureFor(ISymbol member)
        {
            switch (member.Kind)
            {
                case SymbolKind.Field:
                    {

                        var field = (IFieldSymbol) member;
                        var typeXml = TypeReferenceXml(field.Type);
                        var accessibilityXml = AccessibilityXml(member.DeclaredAccessibility);
                        return $@"
{accessibilityXml}
{typeXml}";
                    }
                case SymbolKind.Method:
                    {
                        var method = (IMethodSymbol)member;
                        var returnXml = method.Name == ".ctor" ? "" : $"<return typeId=\"{method.ReturnType.Id()}\" typeName=\"{method.ReturnType.ToDisplayString()}\"/>";
                        var accessibilityXml = AccessibilityXml(member.DeclaredAccessibility);
                        return $@"
{accessibilityXml}
{returnXml}
<parameters>{ParametersSignature(method.Parameters)}</parameters>
{TypeParametersXmlForDeclaration(method.TypeParameters)}";
                    }

                case SymbolKind.Property:
                    {
                        var property = (IPropertySymbol)member;
                        var typeXml = TypeReferenceXml(property.Type);
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
{AccessibilityXml(member.DeclaredAccessibility)}
{typeXml}
{accessorsXml}
<parameters>{ParametersSignature(property.Parameters)}</parameters>";
                    }

                case SymbolKind.Event:
                    {
                        return $@"
{AccessibilityXml(member.DeclaredAccessibility)}
{TypeReferenceXml(((IEventSymbol)member).Type)}";
                    }
                default:
                    throw new NotImplementedException($"Unsupported type {member.Kind} : {member.Name}");
            }
        }

        private static string TypeParametersXmlForDeclaration(ImmutableArray<ITypeParameterSymbol> typeParameters)
        {
            if (typeParameters.IsEmpty)
                return "";

            return $@"<typeParameters>
{string.Join((string) "\n", typeParameters.Select(p => TypeParameterXml(p, true, false)))}
</typeParameters>";
        }

        private static string AccessibilityXml(Accessibility accessibility)
        {
            return $@"<accessibility>{accessibility}</accessibility>";
        }

        private static string TypeReferenceXml(ITypeSymbol typeSymbol, bool includeConstraints = false)
        {
            var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                return TypeReferenceXml(namedTypeSymbol);
            }

            var sourceTypeParameterSymbol = typeSymbol as ITypeParameterSymbol;
            if (sourceTypeParameterSymbol != null)
            {
                return TypeParameterXml(sourceTypeParameterSymbol, includeConstraints, true);
            }
            throw new NotImplementedException("Unsupported typeSymbol");
        }

        private static string TypeParameterXml(ITypeParameterSymbol sourceTypeParameterSymbol, bool includeConstraints, bool includeDeclaringTypeId)
        {
            string constraintAttributes = "";
            string suffix = "/>";
            if (includeConstraints)
            {
                if (sourceTypeParameterSymbol.HasConstructorConstraint)
                    constraintAttributes += @" hasConstructorConstraint=""true""";
                if (sourceTypeParameterSymbol.HasReferenceTypeConstraint)
                    constraintAttributes += @" hasReferenceTypeConstraint=""true""";
                if (sourceTypeParameterSymbol.HasValueTypeConstraint)
                    constraintAttributes += @" hasValueTypeConstraint=""true""";

                if (!sourceTypeParameterSymbol.ConstraintTypes.IsEmpty)
                {
                    suffix = $@">
{string.Join("\n", sourceTypeParameterSymbol.ConstraintTypes.Select(c => TypeReferenceXml(c)))}
</typeParameter>";
                }
            }

            string declaringTypeId = "";
            if (includeDeclaringTypeId)
                declaringTypeId = $@" declaringTypeId=""{sourceTypeParameterSymbol.DeclaringType.Id()}""";

            return $@"<typeParameter{declaringTypeId} name=""{sourceTypeParameterSymbol.Name}""{constraintAttributes}{suffix}";
        }

        private static string TypeReferenceXml(INamedTypeSymbol namedTypeSymbol)
        {
            var typeTagAttributes =
                $"typeId=\"{namedTypeSymbol.Id()}\" typeName=\"{EscapeXml(namedTypeSymbol.ToDisplayString())}\"";

            if (namedTypeSymbol.IsGenericType)
            {
                var typeArguments = TypeArguments(namedTypeSymbol.TypeArguments);

                return
                    $@"<type {typeTagAttributes}>
    {typeArguments}
    </type>";
            }
            else
                return $"<type {typeTagAttributes}/>";
        }

        private static string TypeArguments(ImmutableArray<ITypeSymbol> typeArguments)
        {
            if (typeArguments.IsEmpty)
                return "";

            string typeArgumentsXml = "";
            foreach (var typeArgument in typeArguments)
                typeArgumentsXml += TypeReferenceXml(typeArgument);

            return  $@"<typeArguments>
{typeArgumentsXml}
</typeArguments>";
        }

        private static string EscapeXml(string xmlString)
        {
            return xmlString
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        private string ParametersSignature(IEnumerable<IParameterSymbol> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                string optionalAttribute = parameter.IsOptional ? @" isOptional=""true""" : "";
                string defaultValueAttribute;
                if (parameter.HasExplicitDefaultValue)
                {
                    string defaultValue;
                    if (parameter.ExplicitDefaultValue == null)
                        defaultValue = "default";
                    else
                        defaultValue = parameter.ExplicitDefaultValue.ToString();

                    defaultValueAttribute = $@" defaultValue=""{defaultValue}""";
                }
                else
                    defaultValueAttribute = "";
                sb.AppendLine($"<parameter name=\"{parameter.Name}\" typeId=\"{parameter.Type.Id()}\" typeName=\"{parameter.Type.ToDisplayString()}\"{optionalAttribute}{defaultValueAttribute}/>");
            }
            return sb.ToString();
        }

        public void SetType(string docXml, params string[] sourcePaths)
        {
            //TODO: Check if we need to visit newly instantiated AST nodes
            //TODO: Extract common roslyn initializion code.
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


            var partialInfoCollector = new PartialTypeInfoCollectorVisitor(docXml);
            foreach (var syntaxTree in syntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                partialInfoCollector.Visit(syntaxTree.GetRoot(), semanticModel);
            }

            var docUpdater = new XmlDocReplacerVisitor(docXml, partialInfoCollector);
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

    public class PartialTypeInfoCollectorVisitor : CSharpSyntaxWalker
    {

        /**
         * 1) Find the right SyntaxNode should be updated (a)
         * 2) Add all other partial SyntaxNode into a list (b)
         * 3) In the *Updater Visitor* take that list and
         *      3.1) When visiting node *a* update the documentation
         *      3.2) When visiting node from *b* just wipe out the xml comments
         */
        private readonly XmlDocument _xmlDoc;
        private SemanticModel _semanticModel;
        private Dictionary<string, SyntaxNode> _documentationTargetTypeNodes = new Dictionary<string, SyntaxNode>();

        public PartialTypeInfoCollectorVisitor(string content)
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(content);
        }

        internal void Visit(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
            Visit(syntaxNode);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            base.VisitClassDeclaration(node);
            DecidePriority(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            base.VisitInterfaceDeclaration(node);
            DecidePriority(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            base.VisitStructDeclaration(node);
            DecidePriority(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            base.VisitEnumDeclaration(node);
            DecidePriority(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            base.VisitMethodDeclaration(node);
            DecidePriority(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
            DecidePriority(node);
        }


        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);

            if (node is BaseFieldDeclarationSyntax baseFieldDeclarationSyntax)
                VisitBaseFieldDeclaration(baseFieldDeclarationSyntax);
            else if (node is BaseTypeDeclarationSyntax 
                     || node is MemberDeclarationSyntax
                     || (node is VariableDeclaratorSyntax && isVisitingField))
                DecidePriority(node);
        }

        private bool isVisitingField = false;
        public void VisitBaseFieldDeclaration(BaseFieldDeclarationSyntax node)
        {
            isVisitingField = true;
            try
            {
                DecidePriority(node, _semanticModel.GetDeclaredSymbol(node.Declaration.Variables[0]));
                base.Visit(node);
            }
            finally
            {
                isVisitingField = false;
            }
        }

        private void DecidePriority(SyntaxNode node)
        {
            var typeSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (typeSymbol == null)
                return;

            DecidePriority(node, typeSymbol);
        }

        private void DecidePriority(SyntaxNode node, ISymbol typeSymbol)
        {
            var id = typeSymbol.Id();
            SyntaxNode existingTargetNode;
            if (_documentationTargetTypeNodes.TryGetValue(id, out existingTargetNode))
            {
                if (CecilUtility.HasXmlDocs(node) && !CecilUtility.HasXmlDocs(existingTargetNode))
                    _documentationTargetTypeNodes[id] = node;
            }
            else
                _documentationTargetTypeNodes[id] = node;
        }

        public bool ShouldThisNodeBeDocumented(SyntaxNode node, ISymbol symbol)
        {
            if (symbol == null)
                return false;

            SyntaxNode targetTypeNode;
            if (!_documentationTargetTypeNodes.TryGetValue(symbol.Id(), out targetTypeNode))
                throw new NotImplementedException("This type of node has not been prioritized yet.");

            return targetTypeNode == node;
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