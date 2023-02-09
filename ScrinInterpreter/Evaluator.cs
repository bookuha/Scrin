using ScrinInterpreter.Parsing;
using ScrinInterpreter.Parsing.Expressions;

namespace ScrinInterpreter;

public class Evaluator : IVisitor<Object>
{

    public object Evaluate(Expression expr)
    {
        return expr.Accept(this);
    }
    public object VisitBinaryExpression(BinaryExpression expression)
    {
        var left = Evaluate(expression.Left);
        var right = Evaluate(expression.Right);

        return expression.Operator.Type switch
        {
            TokenType.Comma => expression.Right, // todo: Left operand might be a function call with side effects
            TokenType.Minus => (double) left - (double) right,
            TokenType.Plus when left is double l && right is double r => l + r,
            TokenType.Plus when left is string l && right is string r => l + r,
            TokenType.Slash => (double) left / (double) right,
            TokenType.Star => (double) left * (double) right,
            TokenType.Greater => (double) left > (double) right,
            TokenType.GreaterEqual => (double) left >= (double) right,
            TokenType.Less => (double) left < (double) right,
            TokenType.LessEqual => (double) left <= (double) right,
            TokenType.BangEqual => !AreEqual(left,right),
            TokenType.EqualEqual => AreEqual(left,right),
            _ => null
        };
    }

    private bool AreEqual(object a, object b)
    {
        if (a is null && b is null) return true;
        if (a is null) return false;

        return a.Equals(b);
    }

    public object VisitGroupingExpression(GroupingExpression expression)
    {
        return Evaluate(expression.Expression);
    }

    public object VisitLiteralExpression(LiteralExpression expression)
    {
        return expression.Value;
    }

    public object VisitUnaryExpression(UnaryExpression expression)
    {
        var res = Evaluate(expression.Right);
        return expression.Operator.Type switch
        {
            TokenType.Bang => !IsTruthy(res),
            TokenType.Minus => -(double)res, // well not always double.
                                  // todo: since we only have the "number" to represent numbers, is always having this as double good enough?
            _ => null
        };
    }

    private bool IsTruthy(object obj)
    {
        if (obj is null) return false;
        if (obj is bool) return (bool)obj;
        return true;
    }



    public object VisitTernaryExpression(TernaryExpression expression)
    {
        throw new NotImplementedException();
    }
}