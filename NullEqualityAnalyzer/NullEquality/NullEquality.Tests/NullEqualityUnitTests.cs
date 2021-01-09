using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = NullEquality.Tests.CSharpCodeFixVerifier<NullEquality.NullEqualityAnalyzer, NullEquality.NullEqualityCodeFixProvider>;

namespace NullEquality.Tests
{
    [TestClass]
    [TestCategory(nameof(NullEquality))]
    public class NullEqualityUnitTest
    {
        [TestMethod]
        public async Task Should_Analyzer_NotFind_CodeSmell()
        {
            var source = @"
namespace TestConsoleApplication
{
    public class TestClass
    {
        public bool TestMethod()
        {
            var obj = new object();
            return obj is null ? true : false;
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
        public bool TestMethod()
        {
            var obj = new object();
            return obj == null ? true : false;
        }
    }
}";

            var sourceFix = @"
namespace TestConsoleApplication
{
    public class TestClass
    {
        public bool TestMethod()
        {
            var obj = new object();
            return obj is null ? true : false;
        }
    }
}";
            const string diagnosticId = nameof(NullEquality);
            var diagnostic = VerifyCS.Diagnostic(diagnosticId)
                .WithSpan(9, 20, 9, 31)
                .WithArguments("obj");

            await VerifyCS.VerifyCodeFixAsync(source, diagnostic, sourceFix);
        }
    }
}
