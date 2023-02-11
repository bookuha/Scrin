namespace ScrinInterpreter.App.Parser.Expressions;

public class LiteralExpression : Expression
{
    public LiteralExpression(object value)
    {
        Value = value;
    }

    public object Value { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}