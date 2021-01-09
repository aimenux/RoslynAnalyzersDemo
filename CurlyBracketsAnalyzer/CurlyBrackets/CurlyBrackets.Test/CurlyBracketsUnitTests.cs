using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = CurlyBrackets.Tests.CSharpCodeFixVerifier<CurlyBrackets.CurlyBracketsAnalyzer,CurlyBrackets.CurlyBracketsCodeFixProvider>;

namespace CurlyBrackets.Tests
{
    [TestClass]
    [TestCategory(nameof(CurlyBrackets))]
    public class CurlyBracketsUnitTest
    {
        [TestMethod]
        public async Task Should_Analyzer_NotFind_CodeSmell()
        {
            var source = @"
namespace TestConsoleApplication
{
    public class TestClass
    {
        public bool TestMethod(int counter)
        {
            if (--counter == 0)
            {
               return true;
            }

            return false;
        }
    }
}";
            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task Should_Fix_CodeSmell()
        {
            var source = @"
namespace TestConsoleApplication
{
    public class TestClass
    {
        public bool TestMethod(int counter)
        {
            if (--counter == 0)
               return true;
            return false;
        }
    }
}";

            var sourceFix = @"
namespace TestConsoleApplication
{
    public class TestClass
    {
        public bool TestMethod(int counter)
        {
            if (--counter == 0)
            {
                return true;
            }

            return false;
        }
    }
}";
            const string diagnosticId = nameof(CurlyBrackets);
            var diagnostic = VerifyCS.Diagnostic(diagnosticId).WithSpan(9, 16, 9, 22);
            await VerifyCS.VerifyCodeFixAsync(source, diagnostic, sourceFix);
        }
    }
}
