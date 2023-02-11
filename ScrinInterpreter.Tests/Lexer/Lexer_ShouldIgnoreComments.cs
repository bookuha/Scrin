using ScrinInterpreter.App.Lexer;

namespace ScrinInterpreter.Tests.Lexer;

[TestFixture("// 123 \n meow")]
[TestFixture("// 333 // // \n meow")]
[TestFixture("// meow \n meow")]
[TestFixture("/* woof */ meow")]
[TestFixture("/**/ meow")]
public class Lexer_ShouldIgnoreComments
{
    [SetUp]
    public void SetUp()
    {
        _lexer = new App.Lexer.Lexer(_testString);
    }

    private App.Lexer.Lexer _lexer;
    private readonly string _testString;

    public Lexer_ShouldIgnoreComments(string testString)
    {
        _testString = testString;
    }

    [Test]
    public void IsParsed_InputIsStringCommentAndIgnored_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result is {Type: TokenType.Identifier, Lexeme: "meow"}
            , Is.True);
    }
}