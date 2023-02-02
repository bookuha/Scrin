namespace ScrinInterpreter.Tests.Lexer;
[TestFixture("// 123 \n meow")]
[TestFixture("// 333 // // \n meow")]
[TestFixture("// meow \n meow")]
[TestFixture("/* woof */ meow")]
[TestFixture("/**/ meow")]
public class Lexer_ShouldIgnoreComments
{
    private ScrinInterpreter.Lexer _lexer;
    private string _testString;

    public Lexer_ShouldIgnoreComments(string testString)
    {
        _testString = testString;
    }

    [SetUp]
    public void SetUp()
    {
        _lexer = new ScrinInterpreter.Lexer(_testString);
    }

    [Test]
    public void IsParsed_InputIsStringCommentAndIgnored_ReturnTrue()
    {
        var result = _lexer.Tokenize()[0];
        Assert.That(
            result is {Type: TokenType.Identifier, Lexeme: "meow"} 
            ,Is.True);
    }
}