namespace ScrinInterpreter.Parsing.Expressions;

public abstract class Expression
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}