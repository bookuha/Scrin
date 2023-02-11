using ScrinInterpreter.App.Parser.Expressions;

namespace ScrinInterpreter.App;

public interface IVisitor<T>
{
    T VisitBinaryExpression(BinaryExpression expression);
    T VisitGroupingExpression(GroupingExpression expression);
    T VisitLiteralExpression(LiteralExpression expression);
    T VisitUnaryExpression(UnaryExpression expression);
    T VisitTernaryExpression(TernaryExpression expression);
}