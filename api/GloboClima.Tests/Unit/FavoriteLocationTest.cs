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
        Assert.Throws<DomainException>(() => new FavoriteLocation()
        {
            Lat = lat,
            Lon = 1,
            Username = "fulano"
        });
    }

    [Theory]
    [InlineData(-190)]
    [InlineData(192)]
    [InlineData(-181)]
    [InlineData(181)]
    public void ShouldNotCreateFavoriteLocationWithInvalidLon(int lon)
    {
        Assert.Throws<DomainException>(() => new FavoriteLocation()
        {
            Lat = 1,
            Lon = lon,
            Username = "fulano"
        });
    }
}