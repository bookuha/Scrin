namespace ScrinInterpreter;

public class Token
{
    public TokenType Type { get; init; }
    public string Lexeme { get; init; }
    public Object? Literal { get; init; }
    public int Line { get; init;  }

    public Token(TokenType type, string lexeme, object? literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        return Type + " " + Lexeme + " " + Literal;
    }
}