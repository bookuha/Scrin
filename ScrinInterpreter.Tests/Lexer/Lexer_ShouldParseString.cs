using NUnit.Framework;

namespace ScrinInterpreter.Tests.Lexer;

[TestFixture]
public class Lexer_ShouldParseString
{
    private ScrinInterpreter.Lexer _lexer;

    [SetUp]
    public void SetUp()
    {
        string test = "\"Meow\"";
        _lexer = new ScrinInterpreter.Lexer(test);
    }

    [Test]
    public void IsParsed_InputIsStringMeow_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result is {Type: TokenType.String, Lexeme: "\"Meow\"", Literal: "Meow"} 
            ,Is.True);
    }
    
}