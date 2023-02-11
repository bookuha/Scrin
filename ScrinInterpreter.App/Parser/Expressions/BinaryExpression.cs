using ScrinInterpreter.App.Lexer;

namespace ScrinInterpreter.App.Parser.Expressions;

public class BinaryExpression : Expression
{
    public BinaryExpression(Expression left, Token @operator, Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public Expression Left { get; init; }
    public Expression Right { get; init; }
    public Token Operator { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}