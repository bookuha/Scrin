using ScrinInterpreter.Parser.Expressions;

namespace ScrinInterpreter.Parser;

public interface IVisitor<T>
{
    T VisitBinaryExpression(BinaryExpression expression);
    T VisitGroupingExpression(GroupingExpression expression);
    T VisitLiteralExpression(LiteralExpression expression);
    T VisitUnaryExpression(UnaryExpression expression);
}