namespace ScrinInterpreter.App.Lexer;

public class Token
{
    public Token(TokenType type, string lexeme, object? literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public TokenType Type { get; init; }
    public string Lexeme { get; init; }
    public object? Literal { get; init; }
    public int Line { get; init; }

    public override string ToString()
    {
        return Type + " " + Lexeme + " " + Literal;
    }
}