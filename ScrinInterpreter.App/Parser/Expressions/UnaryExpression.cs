using ScrinInterpreter.App.Lexer;

namespace ScrinInterpreter.App.Parser.Expressions;

public class UnaryExpression : Expression
{
    public UnaryExpression(Token @operator, Expression right)
    {
        Operator = @operator;
        Right = right;
    }

    public Token Operator { get; init; }
    public Expression Right { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}