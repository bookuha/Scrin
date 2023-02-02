namespace ScrinInterpreter;

public class BinaryExpression : Expression
{
    private Expression _left;
    private Expression _right;
    private Token _operator;
    public BinaryExpression(Expression left, Expression right, Token @operator)
    {
        _left = left;
        _right = right;
        _operator = @operator;
    }
}