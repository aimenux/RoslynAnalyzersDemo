using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace NullEquality
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NullEqualityAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "CodeSmells";
        public const string DiagnosticId = nameof(NullEquality);

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeOperation, OperationKind.Conditional);
        }

        private static void AnalyzeOperation(OperationAnalysisContext context)
        {
            var expression = context.GetBinaryExpressionSyntax();
            var identifierName = expression.GetIdentifierNameSyntax();
            if (identifierName != null)
            {
                var location = expression.GetLocation();
                var argument = identifierName.Identifier.ValueText;
                var diagnostic = Diagnostic.Create(Rule, location, argument);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
