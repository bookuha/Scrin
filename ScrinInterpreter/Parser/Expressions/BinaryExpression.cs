namespace ScrinInterpreter.Parser.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left { get; init; }
    public Expression Right { get; init; }
    public Token Operator { get; init; }
    public BinaryExpression(Expression left, Expression right, Token @operator)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}