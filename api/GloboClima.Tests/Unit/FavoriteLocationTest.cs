namespace GloboClima.Tests.Unit;

public class FavoriteLocationTest
{
    [Theory]
    [InlineData(-102)]
    [InlineData(137)]
    [InlineData(-91)]
    [InlineData(91)]
    public void ShouldNotCreateFavoriteLocationWithInvalidLat(int lat)
    {
        Assert.Throws<DomainException>(() => new FavoriteLocation() { Lat = lat, Lon = 1 });
    }
}