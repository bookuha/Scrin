using ScrinInterpreter;

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