using ScrinInterpreter.App.Lexer;

namespace ScrinInterpreter.Tests.Lexer;

[TestFixture("123")]
[TestFixture("121134")]
[TestFixture("1.23")]
[TestFixture("0.11")]
[TestFixture("0")]
public class Lexer_ShouldParseNumber
{
    [SetUp]
    public void SetUp()
    {
        _lexer = new App.Lexer.Lexer(_testString);
    }

    private App.Lexer.Lexer _lexer;
    private readonly string _testString;

    public Lexer_ShouldParseNumber(string testString)
    {
        _testString = testString;
    }

    [Test]
    public void IsParsed_InputIsNumber_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result.Type == TokenType.Number && result.Lexeme == _testString && result.Literal == _testString
            , Is.True);
    }
}