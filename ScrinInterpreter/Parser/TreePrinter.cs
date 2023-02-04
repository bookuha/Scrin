using System.Text;
using ScrinInterpreter.Parser.Expressions;

namespace ScrinInterpreter.Parser;

public class TreePrinter : IVisitor<string>
{
    public string Print(Expression expression)
    {
        return expression.Accept(this);
    }

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

    private string Parenthesize(string name, params Expression[] expressions)
    {
        var result = new StringBuilder();
        
        foreach (var expr in expressions)
        {
            result.Append(expr.Accept(this));
            result.Append(" ");
        }

        result.Append(name);

        return result.ToString();
    }
}   