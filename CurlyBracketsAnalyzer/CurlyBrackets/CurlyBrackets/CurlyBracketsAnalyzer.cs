using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace CurlyBrackets
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CurlyBracketsAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "CodeSmells";
        public const string DiagnosticId = nameof(CurlyBrackets);

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(syntaxTreeContext =>
            {
                var root = syntaxTreeContext.Tree.GetRoot(syntaxTreeContext.CancellationToken);
                foreach (var statement in root.DescendantNodes().OfType<StatementSyntax>())
                {
                    if (statement is BlockSyntax)
                    {
                        continue;
                    }

                    if (statement.Parent is StatementSyntax && !(statement.Parent is BlockSyntax))
                    {
                        var diagnostic = Diagnostic.Create(Rule, statement.GetFirstToken().GetLocation());
                        syntaxTreeContext.ReportDiagnostic(diagnostic);
                    }
                }
            });
        }
    }
}
