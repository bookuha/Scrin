using System.Text;
using ScrinInterpreter.App.Parser.Expressions;

namespace ScrinInterpreter.App;

public class TreePrinter : IVisitor<string>
{
    public string VisitBinaryExpression(BinaryExpression expression)
    {
        return Parenthesize(expression.Operator.Lexeme, expression.Left, expression.Right);
    }

    public string VisitGroupingExpression(GroupingExpression expression)
    {
        return Parenthesize("group", expression.Expression);
    }

    public string VisitLiteralExpression(LiteralExpression expression)
    {
        return Parenthesize(expression.Value.ToString() ?? "nil");
    }

    public string VisitUnaryExpression(UnaryExpression expression)
    {
        return Parenthesize(expression.Operator.Lexeme, expression.Right);
    }

    public string VisitTernaryExpression(TernaryExpression expression)
    {
        return Parenthesize("?:", expression.Expression, expression.LeftResult, expression.RightResult);
    }

    public string Print(Expression expression)
    {
        return expression.Accept(this);
    }

    private string Parenthesize(string name, params Expression[] expressions)
    {
        var result = new StringBuilder();
        result.Append("(").Append(name);
        foreach (var expr in expressions)
        {
            result.Append(expr.Accept(this));
            result.Append(" ");
        }

        result.Append(")");

        return result.ToString();
    }
}