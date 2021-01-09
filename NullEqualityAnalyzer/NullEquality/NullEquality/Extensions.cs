using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace NullEquality
{
    public static class Extensions
    {
        public static BinaryExpressionSyntax GetBinaryExpressionSyntax(this OperationAnalysisContext context)
        {
            var operation = context.Operation as IConditionalOperation;
            return operation?.Condition?.Syntax as BinaryExpressionSyntax;
        }

        public static IdentifierNameSyntax GetIdentifierNameSyntax(this BinaryExpressionSyntax expression)
        {
            if (expression?.Kind() != EqualsExpression)
            {
                return null;
            }

            if (expression.Left is LiteralExpressionSyntax left && left.Kind() == NullLiteralExpression)
            {
                return expression.Right as IdentifierNameSyntax;
            }

            if (expression.Right is LiteralExpressionSyntax right && right.Kind() == NullLiteralExpression)
            {
                return expression.Left as IdentifierNameSyntax;
            }

            return null;
        }
    }
}
