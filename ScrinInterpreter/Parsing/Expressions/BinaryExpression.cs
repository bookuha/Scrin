namespace ScrinInterpreter.Parsing.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left { get; init; }
    public Expression Right { get; init; }
    public Token Operator { get; init; }

    public BinaryExpression(Expression left, Token @operator, Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}