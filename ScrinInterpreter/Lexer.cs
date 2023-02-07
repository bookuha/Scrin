namespace ScrinInterpreter;

public class Lexer
{
    private string _source; // Should it even have a state?
    private List<Token> _tokens = new List<Token>();

    private int _start;
    private int _current;
    private int _line;

    private Scrin? _scrin { get; set; }

    public Lexer(string source)
    {
        _source = source;
    }

    public Lexer(string source, Scrin scrin)
    {
        _source = source;
        _scrin = scrin;
    }

    public List<Token> Tokenize()
    {
        // Start reading characters
        // If group of characters form a supported token, then parse it and add to the token list

        while (!IsAtEnd())
        {
            _start = _current;

            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return _tokens;
    }

    private void ScanToken()
    {
        char character = Step();

        switch (character) // Switch to pattern matching
        {
            case '(':
                PushToken(
                    MatchCharacter(')') ? TokenType.Fun : TokenType.LeftParen
                );
                break;
            case ')':
                PushToken(TokenType.RightParen);
                break;
            case '{':
                PushToken(TokenType.LeftBrace);
                break;
            case '}':
                PushToken(TokenType.RightBrace);
                break;
            case ',':
                PushToken(TokenType.Comma);
                break;
            case '.':
                PushToken(TokenType.Dot);
                break;
            case '-':
                PushToken(TokenType.Minus);
                break;
            case '+':
                PushToken(TokenType.Plus);
                break;
            case ';':
                PushToken(TokenType.Semicolon);
                break;
            case '*':
                PushToken(TokenType.Star);
                break;
            case '!':
                PushToken(
                    MatchCharacter('=') ? TokenType.BangEqual : TokenType.Bang
                );
                break;
            case '=':
                PushToken(
                    MatchCharacter('=') ? TokenType.EqualEqual : TokenType.Equal
                );
                break;
            case '<':
                PushToken(
                    MatchCharacter('=') ? TokenType.LessEqual : TokenType.Less
                );
                break;
            case '>':
                PushToken(
                    MatchCharacter('=') ? TokenType.GreaterEqual : TokenType.Greater
                );
                break;
            case '/':
                if (MatchCharacter('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                        Step();
                }
                else if (MatchCharacter('*'))
                {
                    while (Peek() != '*' && PeekNext() != '/') // nested comment wont work
                    {
                        if (Peek() == '\n')
                            _line++;
                        Step();
                    }

                    Step(); // Looks stupid. Check it in LENS
                    Step();
                }
                else
                    PushToken(TokenType.Slash);

                break;

            #region ignorecharacters

            case ' ': break;
            case '\r': break;
            case '\t': break;

            case '\n':
                _line++;
                break;

            #endregion

            case '"':
                PushStringToken(); // name it better
                break;

            default:
                if (Char.IsLetter(character))
                {
                    PushIdentifierToken();
                }

                else if (Char.IsDigit(character))
                {
                    if (character is '0' && Char.IsDigit(Peek())) // better move it to PushNumberToken i guess
                    {
                        ReportError(_line, "Integer number must not start with 0");
                        break;
                    }

                    PushNumberToken();
                }
                else
                {
                    ReportError(_line, "Unexpected character", location: character.ToString());
                }

                break;
        }
    }

    private void PushIdentifierToken()
    {
        while (Char.IsLetterOrDigit(Peek())) Step();

        string text = _source.Substring(_start, _current - _start);
        bool valueFound = _keywords.TryGetValue(text, out TokenType type);
        if (!valueFound) type = TokenType.Identifier;

        PushToken(type);
    }

    private static Dictionary<string, TokenType> _keywords = new()
    {
        ["and"] = TokenType.And,
        ["class"] = TokenType.Class,
        ["else"] = TokenType.Else,
        ["false"] = TokenType.False,
        ["for"] = TokenType.For,
        ["fun"] = TokenType.Fun,
        ["if"] = TokenType.If,
        ["nil"] = TokenType.Nil,
        ["or"] = TokenType.Or,
        ["print"] = TokenType.Print,
        ["return"] = TokenType.Return,
        ["super"] = TokenType.Super,
        ["this"] = TokenType.This,
        ["true"] = TokenType.True,
        ["var"] = TokenType.Var,
        ["while"] = TokenType.While,
    };

    private void ReportError(int line, string errorMessage, string location = "")
    {
        if (_scrin is not null)
            _scrin.ReportLexError(_line, location, errorMessage);
        else
            Console.WriteLine("No Scrin instance has been set, therefore no LEXING error has been displayed");
    }

    private bool MatchCharacter(char character)
    {
        if (IsAtEnd()) return false;

        if (_source[_current] == character) // simplify
        {
            _current++;
            return true;
        }

        else return false;
    }

    private void PushToken(TokenType type)
    {
        PushToken(type, null);
    }

    private void PushToken(TokenType type, Object literal)
    {
        string
            text = _source.Substring(_start,
                _current - _start); // n-char token = number of symbols between start reading and finish reading pointers
        _tokens.Add(new Token(type, text, literal, _line));
    }

    private char Step()
    {
        return _source[_current++];
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 == _source.Length) return '\0'; // kind of IsAtEnd(). Will be refactored.
        return _source[_current + 1];
    }

    private bool IsAtEnd()
    {
        return _current == _source.Length;
    }

    private void PushStringToken()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++; // I dont like this
            Step();
        }

        if (IsAtEnd())
        {
            _scrin?.ReportError(_line, "Here", "Unterminated string."); // handle null
            return;
        }

        Step(); // otherwise start is going to point at the " TEST THIS

        string value = _source.Substring(_start + 1, (_current - _start) - 2); // _start + 1 to avoid "
        PushToken(TokenType.String, value);
    }

    private void PushNumberToken()
    {
        // Handle 0 and 0.n
        while (Char.IsDigit(Peek())) Step();

        if (Peek() == '.' && Char.IsDigit(PeekNext()))
        {
            Step();

            while (Char.IsDigit(Peek())) Step();

            // and if we met another . ? well, this is fine since we dont have our primitives as classes
        }

        PushToken(TokenType.Number, _source.Substring(_start, _current - _start));
    }
}