namespace ScrinInterpreter.App.Parser.Expressions;

public class TernaryExpression : Expression
{
    public TernaryExpression(Expression expression, Expression leftResult, Expression rightResult)
    {
        Expression = expression;
        LeftResult = leftResult;
        RightResult = rightResult;
    }

    public Expression Expression { get; init; }
    public Expression LeftResult { get; init; }
    public Expression RightResult { get; init; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitTernaryExpression(this);
    }
}