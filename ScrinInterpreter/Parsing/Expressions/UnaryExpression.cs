namespace ScrinInterpreter.Parsing.Expressions;

public class UnaryExpression : Expression
{
    public Token Operator { get; init; }
    public Expression Right { get; init; }

    public UnaryExpression(Token @operator, Expression right)
    {
        Operator = @operator;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}