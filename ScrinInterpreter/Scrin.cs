namespace ScrinInterpreter;

public class Scrin
{
    private Lexer _lexer;
    private bool isFaulted = false;

    public Scrin()
    {
    }
    
    
    public void ExecuteFromFile(string path)
    {
        string script = System.IO.File.ReadAllText(path);
        Execute(script);
        if(isFaulted) Environment.Exit(65);
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
    }

    public void ReportError(int line, string location, string errorMessage) // move to ErrorReporter class
    {
        Console.WriteLine("[line " + line + "] " + location + " Error: " + errorMessage);
        isFaulted = true;
    }
}