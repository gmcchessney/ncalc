using NCalc.Exceptions;
using NCalc.Factories;

namespace NCalc.Tests;

[Trait("Category","Exceptions")]
public class ExceptionsTests
{
    [Theory]
    [InlineData("ifs()")]
    [InlineData("ifs([divider] > 0)")]
    [InlineData("ifs([divider] > 0, [divider] / [divided])")]
    [InlineData("ifs([divider] > 0, [divider] / [divided], [divider < 0], [divider] + [divided])")]
    public void Ifs_With_Improper_Arguments_Should_Throw_Exceptions(string expression)
    {
        Assert.Throws<ArgumentException>(() => new Expression(expression).Evaluate());
    }

    [Theory]
    [InlineData(". + 2")]
    [InlineData("(3 + 2")]
    public void Should_Throw_Parse_Exception(string expression)
    {
        Assert.Throws<NCalcParserException>(() =>new Expression(expression).Evaluate());
    }
    
    [Fact]
    public void Should_Detect_Syntax_Errors_Before_Evaluation()
    {
        //Yes, I know at v3 the test was a + b * ( , but it's impossible to define an empty expression.
        var e = new Expression("a + b * ( 1 + 1");
        Assert.Null(e.Error);
        Assert.True(e.HasErrors());
        Assert.NotNull(e.Error);

        e = new Expression("* b ");
        Assert.Null(e.Error);
        Assert.True(e.HasErrors());
        Assert.NotNull(e.Error);
    }
    
    [Fact]
    public void Should_Display_Error_If_UncompatibleTypes()
    {
        var e = new Expression("(a > b) + 10")
        {
            Parameters =
            {
                ["a"] = 1,
                ["b"] = 2
            }
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            e.Evaluate();
        });
    }
    
    [Fact]
    public void Should_Throw_Exception_On_Lexer_Errors_Issue_6()
    {
        Assert.Throws<NCalcParserException>(() => LogicalExpressionFactory.Create("#t -chers", ExpressionOptions.NoCache));

        var invalidDateException = Assert.Throws<NCalcParserException>(() => LogicalExpressionFactory.Create("#13/13/2222#", ExpressionOptions.NoCache));
        Assert.IsType<FormatException>(invalidDateException.InnerException);

        //At v4, DateTime is better handled, and this should no longer cause errors.
        // https://github.com/ncalc/ncalc-async/issues/6
        try
        {
            LogicalExpressionFactory.Create("Format(\"{0:(###) ###-####}\", \"9999999999\")");
        }
        catch
        {
            Assert.Fail("No exception should be thrown here.");
        }
    }
}