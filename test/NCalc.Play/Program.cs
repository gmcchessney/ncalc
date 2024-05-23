using NCalc;
using NCalc.Factories;
using NCalc.Play;

var logicalExpression = LogicalExpressionFactory.Create("Repeat([value] > 10, 3)");
logicalExpression.Accept(new ExpressionReParserVisitor());

var e = new Expression(logicalExpression);

var times = new Dictionary<string, int>() { };

e.EvaluateFunction += (name, args) =>
{
    if (name == "Repeat")
    {
        var t = (int)args.Parameters[1].Evaluate() - 1;
        var r = (bool)args.Parameters[0].Evaluate();
        var id = (string)args.Parameters[2].Evaluate();
        if (r && id != null)
        {
            if (!times.ContainsKey(id))
            {
                times[id] = t;
            }
            else
            {
                times[id]--;
            }
        }

        args.Result = r && times[id] == 0;
    }
};

e.Parameters["value"] = 9;
if ((bool)e.Evaluate() != false)
    throw new Exception();

e.Parameters["value"] = 11;
if ((bool)e.Evaluate() != false)
    throw new Exception();

e.Parameters["value"] = 12;
if ((bool)e.Evaluate() != false)
    throw new Exception();

e.Parameters["value"] = 13;
if ((bool)e.Evaluate() != true)
    throw new Exception();

