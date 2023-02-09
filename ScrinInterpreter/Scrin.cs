using ScrinInterpreter.Parsing;

namespace ScrinInterpreter;

public class Scrin
{
    private Lexer _lexer;
    private Parser _parser;
    private bool _isFaulted;
    

    public void ExecuteFromFile(string path)
    {
        var script = File.ReadAllText(path);
        Execute(script);
        if (_isFaulted) Environment.Exit(65);
    }

    public void ExecuteLineFromPrompt()
    {
        while (true)
        {
            Console.Write(">");
            string? line = Console.ReadLine();
            if (line is null) break;
            Execute(line);
            _isFaulted = false;
        }
    }

    private void Execute(string script)
    {
        Console.WriteLine("<// Lexing //>");
        _lexer = new Lexer(script, this);
        List<Token> tokens = _lexer.Tokenize();
        foreach (var token in tokens)
        {
            Console.WriteLine(token.ToString());
        }
        
        Console.WriteLine("<// Parsing //>");
        _parser = new Parser(tokens, this);
        var testTree = _parser.Parse();

        if(_isFaulted) {
            Console.WriteLine("Error: The process was faulted.");
        }
        else
        {
            var myVisitor = new TreePrinter();
            var result = myVisitor.Print(testTree);
            Console.WriteLine(result);

            var myEvaluator = new Evaluator();
            var evalResult = myEvaluator.Evaluate(testTree);
            Console.WriteLine(evalResult);
        }
    }

    public void ReportError(int line, string location, string errorMessage) // move to ErrorReporter class
    {
        Console.WriteLine("[line " + line + "] " + location + " Error: " + errorMessage);
        _isFaulted = true;
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