namespace GloboClima.Tests.Unit;

public class FavoriteCountryTest
{
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("test")]
    [InlineData("ab1")]
    [InlineData("a12")]
    [InlineData("123")]
    [InlineData("ab.")]
    [InlineData("@ab")]
    public void ShouldNotCreateFavoriteCountryWithInvalidCioc(string cioc)
    {
        Assert.Throws<DomainException>(() => new FavoriteCountry() { Cioc = cioc });
    }
}