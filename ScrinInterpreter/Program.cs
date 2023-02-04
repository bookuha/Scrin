using ScrinInterpreter;
using ScrinInterpreter.Parser;
using ScrinInterpreter.Parser.Expressions;

BinaryExpression test = new BinaryExpression(
    new BinaryExpression(
        new LiteralExpression(1),
        new LiteralExpression(2),
         new Token(TokenType.Plus, "+", null, 0)
    ),
    new BinaryExpression(
        new LiteralExpression(4),
        new LiteralExpression(3),
        new Token(TokenType.Minus, "-", null, 0)),
    new Token(TokenType.Star, "*", null, 0)
); // involving member initializer list would make it prettier

var myVisitor = new TreePrinter();
var result = myVisitor.Print(test);
Console.WriteLine(result); // [ (1 + 2) * (4 - 3) ] => [ 1 2 + 4 3 - * ]


if (args.Length > 1)
{
    Console.WriteLine("Use scrin [script]");
    Environment.Exit(64);
}

else if (args.Length == 1)
{
    Scrin scrin = new Scrin();
    scrin.ExecuteFromFile(args[0]);
}
else
{
    Scrin scrin = new Scrin();
    scrin.ExecuteLineFromPrompt();
}