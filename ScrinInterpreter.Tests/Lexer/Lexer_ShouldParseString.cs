using ScrinInterpreter.App.Lexer;

namespace ScrinInterpreter.Tests.Lexer;

[TestFixture]
public class Lexer_ShouldParseString
{
    [SetUp]
    public void SetUp()
    {
        var test = "\"Meow\"";
        _lexer = new App.Lexer.Lexer(test);
    }

    private App.Lexer.Lexer _lexer;

    [Test]
    public void IsParsed_InputIsStringMeow_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result is {Type: TokenType.String, Lexeme: "\"Meow\"", Literal: "Meow"}
            , Is.True);
    }
}