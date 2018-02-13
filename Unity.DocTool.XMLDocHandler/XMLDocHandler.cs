﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.DocTool.XMLDocHandler.Extensions;

namespace Unity.DocTool.XMLDocHandler
{
    public class XMLDocHandler
    {
        public void GetComments(string filePath)
        {
            IEnumerable<string> defines = new string[0];
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse,
                SourceCodeKind.Regular, defines);

            var syntaxTree =
                SyntaxFactory.ParseSyntaxTree(File.ReadAllText(filePath), parserOptions, Path.GetFileName(filePath));

            //var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            //compilerOptions = compilerOptions.WithAllowUnsafe(true);
            //var compilation = CSharpCompilation.Create("Test", new[] {syntaxTree}, new MetadataReference[0], compilerOptions);

            var visitor = new XMLDocExtractVisitor("");
            visitor.Visit(syntaxTree.GetRoot());
        }

        public void UpdateComments(string filePath)
        {
            IEnumerable<string> defines = new string[0];
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse,
                SourceCodeKind.Regular, defines);

            var syntaxTree =
                SyntaxFactory.ParseSyntaxTree(File.ReadAllText(filePath), parserOptions, Path.GetFileName(filePath));

            //var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            //compilerOptions = compilerOptions.WithAllowUnsafe(true);
            //var compilation = CSharpCompilation.Create("Test", new[] {syntaxTree}, new MetadataReference[0], compilerOptions);

            var visitor = new XMLDocReplacerVisitor();
            var x = visitor.Visit(syntaxTree.GetRoot());
            Console.WriteLine(x);
        }

        public string GetTypesXml(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Directory \"{directoryPath}\" does not exist.");

            IEnumerable<string> defines = new string[0];
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse,
                SourceCodeKind.Regular, defines);
            var filePaths = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
            var syntaxTrees = filePaths.Select(
                p => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(p), parserOptions, p.Substring(directoryPath.Length))).ToArray();

            var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            compilerOptions = compilerOptions.WithAllowUnsafe(true);
            var compilation = CSharpCompilation.Create("Test", syntaxTrees, new MetadataReference[0], compilerOptions);

            var getTypesVisitor = new GetTypesVisitor();
            foreach (var syntaxTree in syntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                getTypesVisitor.Visit(syntaxTree.GetRoot(), semanticModel);
            }

            return getTypesVisitor.GetXml();
        }

        public string GetTypeDocumentation(string id, string rootPath, params string[] paths)
        {
            IEnumerable<string> defines = new string[0];
            var parserOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, DocumentationMode.Parse, SourceCodeKind.Regular, defines);

            var syntaxTrees = paths.Select(p => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(Path.Combine(rootPath, p)), parserOptions, p));

            var compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            compilerOptions = compilerOptions.WithAllowUnsafe(true);
            var compilation = CSharpCompilation.Create("Test", syntaxTrees, new MetadataReference[0], compilerOptions);

            var getTypesVisitor = new XMLDocExtractVisitor(id);
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);

                var desencentes = syntaxTree.GetRoot().DescendantNodes();
                var res = desencentes.OfType<BaseTypeDeclarationSyntax>().ToArray();
                foreach (var typeDeclaration in res)
                {
                    var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                    if (id == typeSymbol.QualifiedName(true, true))
                    {
                        var xml = new StringBuilder($@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
     <doc version=""3"">
         <member name=""{typeSymbol.Name}"" type = ""{typeSymbol.TypeKind}"" namespace=""{typeSymbol.ContainingNamespace}"" inherits=""{typeSymbol.BaseType}"">
        <section>
        {typeSymbol.GetDocumentationCommentXml()}
        </section>");

                        var members = typeSymbol.GetMembers();
                        foreach (var member in members)
                        {
                            xml.Append($@"<member name = ""{member.Name}"" type=""{member.Kind}"">
            <section>
                <signature><![CDATA[{member.ToDisplayString()}]]></signature>
                {member.GetDocumentationCommentXml()}
            </section>
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

    internal class XMLDocReplacerVisitor : CSharpSyntaxRewriter
    {
        public XMLDocReplacerVisitor() : base(true)
        {
        }

        public override SyntaxNode VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            var documentationTarget = node.ParentTrivia.Token.Parent;
            var t = CSharpSyntaxTree.ParseText(
                $"/// <example>{documentationTarget.Kind()}</example>\r\n///<remarks>{documentationTarget.Language}</remarks>\r\n");

            //var t = CSharpSyntaxTree.ParseText($"/// <example>{documentationTarget.Kind()}</example>");

            var root = t.GetRoot();
            return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
                .WithLeadingTrivia(root.GetLeadingTrivia());

            //var result = base.VisitDocumentationCommentTrivia(node);
            //return result
            //    .WithTrailingTrivia(node.GetLeadingTrivia())
            //    .WithTrailingTrivia(t.GetRoot().GetLeadingTrivia());

            //var documentationTarget = node.ParentTrivia.Token.Parent;
            //var t = CSharpSyntaxTree.ParseText($"/// <example>{documentationTarget.Kind()}</example>\r\n");
            //var result = base.VisitDocumentationCommentTrivia(node);
            //return result
            //    .WithTrailingTrivia(node.GetLeadingTrivia())
            //    .WithTrailingTrivia(t.GetRoot().GetLeadingTrivia());
        }
    }
}