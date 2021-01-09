using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace NullEquality
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NullEqualityCodeFixProvider)), Shared]
    public class NullEqualityCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(NullEqualityAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<BinaryExpressionSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: _ => UseIsKeywordAsync(context.Document, declaration),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> UseIsKeywordAsync(Document document, ExpressionSyntax expression)
        {
            var binaryExpression = expression as BinaryExpressionSyntax;
            var identifierName = binaryExpression.GetIdentifierNameSyntax();
            var literalExpression = SyntaxFactory.LiteralExpression(NullLiteralExpression);
            var constantPattern = SyntaxFactory.ConstantPattern(literalExpression);
            var newSyntax = SyntaxFactory.IsPatternExpression(identifierName, constantPattern);
            var root = await document.GetSyntaxRootAsync();
            var node = root.ReplaceNode(expression, newSyntax);
            return document.WithSyntaxRoot(node);
        }
    }
}
