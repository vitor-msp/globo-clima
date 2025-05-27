namespace GloboClima.Tests.Unit;

public class FavoriteLocationTest
{
    [Theory]
    [InlineData(-102.021312)]
    [InlineData(137.156151)]
    [InlineData(-91.66)]
    [InlineData(91.0)]
    public void ShouldNotCreateFavoriteLocationWithInvalidLat(double lat)
    {
        Assert.Throws<DomainException>(() => new FavoriteLocation()
        {
            Lat = lat,
            Lon = 1,
            Username = "fulano"
        });
    }

    [Theory]
    [InlineData(-190.01)]
    [InlineData(192.12)]
    [InlineData(-181.15697)]
    [InlineData(181)]
    public void ShouldNotCreateFavoriteLocationWithInvalidLon(double lon)
    {
        Assert.Throws<DomainException>(() => new FavoriteLocation()
        {
            Lat = 1,
            Lon = lon,
            Username = "fulano"
        });
    }
}