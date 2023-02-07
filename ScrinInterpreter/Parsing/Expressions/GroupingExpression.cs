namespace ScrinInterpreter.Parsing.Expressions;

public class GroupingExpression : Expression
{
    public Expression Expression { get; init; }

    public GroupingExpression(Expression expression)
    {
        Expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpression(this);
    }
}