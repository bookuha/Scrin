using ScrinInterpreter.Parsing;

namespace ScrinInterpreter;

public class Scrin
{
    private Lexer _lexer;
    private Parser _parser;
    private bool isFaulted = false;

    public Scrin()
    {
    }


    public void ExecuteFromFile(string path)
    {
        var script = File.ReadAllText(path);
        Execute(script);
        if (isFaulted) Environment.Exit(65);
    }

    public void ExecuteLineFromPrompt()
    {
        while (true)
        {
            Console.Write(">");
            string? line = Console.ReadLine();
            if (line is null) break;
            Execute(line);
            isFaulted = false;
        }
    }

    public void Execute(string script)
    {
        _lexer = new Lexer(script, this);
        List<Token> tokens = _lexer.Tokenize();
        foreach (var token in tokens)
        {
            Console.WriteLine(token.ToString());
        }

        _parser = new Parser(tokens, this);
        var test = _parser.Parse();

        var myVisitor = new TreePrinter();
        var result = myVisitor.Print(test);
        Console.WriteLine(result);
    }

    public void ReportError(int line, string location, string errorMessage) // move to ErrorReporter class
    {
        Console.WriteLine("[line " + line + "] " + location + " Error: " + errorMessage);
        isFaulted = true;
    }

    public void ReportLexError(int line, string location, string errorMessage) // move to ErrorReporter class
    {
        ReportError(line, location, errorMessage);
    }

    public void ReportParseError(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
            ReportError(token.Line, "at the end", message);
        else
            ReportError(token.Line, "at '" + token.Lexeme + "'", message);
    }
}