using NCalc.Domain;
using NCalc.Visitors;

namespace NCalc.Play;

internal sealed class ExpressionReParserVisitor : LogicalExpressionVisitor
{
    private int _currentFunctionId = 0;

    public override void Visit(Identifier function)
    {
    }

    public override void Visit(UnaryExpression expression)
    {
        expression.Expression.Accept(this);
    }

    public override void Visit(BinaryExpression expression)
    {
        expression.LeftExpression.Accept(this);
        expression.RightExpression.Accept(this);
    }

    public override void Visit(TernaryExpression expression)
    {
        expression.LeftExpression.Accept(this);
        expression.MiddleExpression.Accept(this);
        expression.RightExpression.Accept(this);
    }

    public override void Visit(Function function)
    {
        foreach (var expression in function.Expressions)
            expression.Accept(this);

        if (function.Identifier.Name == "Repeat")
            function.Expressions = function.Expressions.Append(new ValueExpression(_currentFunctionId++.ToString())).ToArray();
    }

    public override void Visit(LogicalExpression expression)
    {
        expression.Accept(this);
    }

    public override void Visit(ValueExpression expression)
    {
    }
}