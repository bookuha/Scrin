using ScrinInterpreter.Parsing.Expressions;

namespace ScrinInterpreter.Parsing;

public interface IVisitor<T>
{
    T VisitBinaryExpression(BinaryExpression expression);
    T VisitGroupingExpression(GroupingExpression expression);
    T VisitLiteralExpression(LiteralExpression expression);
    T VisitUnaryExpression(UnaryExpression expression);
    T VisitTernaryExpression(TernaryExpression expression);
}