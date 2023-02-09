using ScrinInterpreter.Parsing.Expressions;

namespace ScrinInterpreter.Parsing;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current;

    private Scrin? _scrin { get; }

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Parser(List<Token> tokens, Scrin scrin)
    {
        _tokens = tokens;
        _scrin = scrin;
    }


    public Expression Parse()
    {
        try
        {
            return TryExpression();
        }
        catch (ParseException exception)
        {
            return null;
        }
    }

    private Expression TryExpression()
    {
        return TryTernaryConditional();
    }

    private bool MatchAny(params TokenType[] types)
    {
        foreach (var type in types)
            if (Check(type))
            {
                Step();
                return true;
            }

        return false;
    }

    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            Step();
            return true;
        }

        return false;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }

    private Token Step()
    {
        if (!IsAtEnd()) _current++;
        return PeekPrevious(); // So we stay in bounds no matter what. Once we're out the EOF will be returned.
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private Token PeekPrevious()
    {
        return _tokens[_current - 1];
    }
    
    private Expression TryComma()
    {
        var expr = TryEquality();
        
        while (Match(TokenType.Comma))
        {
            var oprt = PeekPrevious();
            var right = TryEquality();
            expr = new BinaryExpression(expr, oprt, right);
        }

        return expr;
    }

    private Expression TryTernaryConditional()
    {
        var expr = TryEquality();

        while (Match(TokenType.QuestionMark))
        {
            
            var left = TryExpression();
            if (Match(TokenType.Semicolon))
            {
                expr = new TernaryExpression(expr, left, TryExpression());
            }
            else EmitError(PeekPrevious(), "Wrong ternary operator");

        }

        return expr;
    }
    
    // So it works looks like that for this case: ( 1==1*2 )
    // Try to evaluate the first token(expression) we meet
    // Having tried each type we eventually decide that this is a primary type expression (literal)  
    // We move the _current pointer further and recursively return control flow to the very beginning checking if next token
    // matches any type. Then in result we have it in our TryEquality() and it appears that the next token actually
    // matches EqualEqual (==), so we try to parse it. We do the very same thing for the right side of the equality expression
    // and it results in the factor expression ( We meet literal, then we check if there is "x" or "/" next (after) to it.
    // Since in our case there is such a token we parse it as a binary factor expression, the right side of which is also a literal
    private Expression TryEquality()
    {
        var expr = TryComparison();

        while (MatchAny(TokenType.BangEqual, TokenType.EqualEqual))
        {
            var oprt = PeekPrevious();
            var right = TryComparison();
            expr = new BinaryExpression(expr, oprt, right);
        }

        return expr;
    }

    private Expression TryComparison()
    {
        var expr = TryTerm();

        while (MatchAny(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
        {
            var oprt = PeekPrevious();
            var right = TryTerm();
            expr = new BinaryExpression(expr, oprt, right);
        }

        return expr;
    }

    private Expression TryTerm() //
    {
        var expr = TryFactor();

        while (MatchAny(TokenType.Plus, TokenType.Minus))
        {
            var oprt = PeekPrevious();
            var right = TryFactor();
            // 1+1+1 will we ever hit later +1 this way? Yes, it will, since we have a while loop here and once
            // second literal is parsed, _current + 1 is called and we are again checking if there is Plus or Minus here
            // I somehow confused it with if statement 
            expr = new BinaryExpression(expr, oprt,
                right); // funny selfpassing strategy I have never encountered before
            // i didn't know expr doesnt get nulled between iterations
        }

        return expr;
    }

    private Expression TryFactor()
    {
        var expr = TryUnary();

        while (MatchAny(TokenType.Star, TokenType.Slash))
        {
            var oprt = PeekPrevious();
            var right = TryUnary();
            expr = new BinaryExpression(expr, oprt, right);
        }

        return expr;
    }

    private Expression TryUnary()
    {
        if (MatchAny(TokenType.Bang, TokenType.Minus))
        {
            var oprt = PeekPrevious();
            var right = TryUnary();
            return new UnaryExpression(oprt, right);
        }

        return TryPrimary();
    }

    private Expression TryPrimary()
    {
        if (Match(TokenType.False)) return new LiteralExpression(false);
        if (Match(TokenType.True)) return new LiteralExpression(true);
        if (Match(TokenType.Nil)) return new LiteralExpression(null);

        if (MatchAny(TokenType.Number, TokenType.String))
            return new LiteralExpression(PeekPrevious().Literal);

        if (Match(TokenType.LeftParen))
        {
            var expr = TryComma();
            Consume(TokenType.RightParen, ")?");
            return new GroupingExpression(expr);
        }

        throw EmitError(Peek(), "Expected expression.");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Step();

        throw EmitError(Peek(), message);
    }

    private ParseException EmitError(Token token, string message)
    {
        if (_scrin is not null)
            _scrin.ReportParseError(token, message);
        else
            Console.WriteLine("No Scrin instance has been set, therefore no error has been displayed");

        return new ParseException();
    }

    private void Synchronize() // We need this to keep parsing even after having errors by jumping out of the statement
    {
        Step();

        while (!IsAtEnd())
        {
            if (PeekPrevious().Type == TokenType.Semicolon) return; // Indicates that we have just stepped out of the statement
        }

        switch (Peek().Type) // Indicates that we are about to jump in the next statement
        {
            case TokenType.Class:
            case TokenType.Fun:
            case TokenType.Var:
            case TokenType.For:
            case TokenType.If:
            case TokenType.While:
            case TokenType.Print:
            case TokenType.Return:
                return;
        }

        Step();
    }
}