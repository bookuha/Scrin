using NUnit.Framework;

namespace ScrinInterpreter.Tests.Lexer;

[TestFixture("123")]
[TestFixture("121134")]
[TestFixture("1.23")]
[TestFixture("0.11")]
[TestFixture("0")]
public class Lexer_ShouldParseNumber
{
    private ScrinInterpreter.Lexer _lexer;
    private string _testString;

    public Lexer_ShouldParseNumber(string testString)
    {
        _testString = testString;
    }

    [SetUp]
    public void SetUp()
    {
        _lexer = new ScrinInterpreter.Lexer(_testString);
    }
    [Test]
    public void IsParsed_InputIsNumber_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result.Type == TokenType.Number && result.Lexeme == _testString && result.Literal == _testString
            ,Is.True);
    }
}