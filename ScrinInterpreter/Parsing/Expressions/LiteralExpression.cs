namespace ScrinInterpreter.Parsing.Expressions;

public class LiteralExpression : Expression
{
    public object Value { get; init; }

    public LiteralExpression(object value)
    {
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}