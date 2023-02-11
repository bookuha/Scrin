namespace ScrinInterpreter.App.Parser.Expressions;

public class GroupingExpression : Expression
{
    public GroupingExpression(Expression expression)
    {
        Expression = expression;
    }

    public Expression Expression { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpression(this);
    }
}