namespace GloboClima.Tests.Unit;

public class TextHasherServiceTest
{
    private readonly TextHasherService _textHasher = new();

    [Fact]
    public void ShouldHashAndVerifyValidPassword()
    {
        var password = "My.123@pass";
        var hash = _textHasher.Hash(password);
        Assert.NotEqual(password, hash);
        Assert.True(_textHasher.Verify(hash, password));
    }

    [Fact]
    public void ShouldVerifyInvalidPassword()
    {
        var password = "My.123@pass";
        var hash = _textHasher.Hash(password);
        Assert.False(_textHasher.Verify(hash, "incorrect-password"));
    }
}