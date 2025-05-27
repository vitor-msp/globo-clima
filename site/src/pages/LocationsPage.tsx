import { useContext, useState } from "react";
import { api, Location } from "../services/api";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";

export const LocationsPage = () => {
  const [currentLat, setCurrentLat] = useState<number>(-19.9191248);
  const [currentLon, setCurrentLon] = useState<number>(-43.9386291);
  const [currentLocation, setCurrentLocation] = useState<Location | null>(null);
  const [isFavorited, setIsFavorited] = useState<boolean>(false);

  const context = useContext(FavoriteLocationsContext);

  const searchLocation = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.getLocationWeatherInformation(
      currentLat,
      currentLon
    );
    if (output.error)
      return alert("Error to get location weather information.");
    setCurrentLocation(output.data);
    checkIfIsFavorited();
  };

  const updateCurrentLat = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentLat(Number(event.target.value));

  const updateCurrentLon = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentLon(Number(event.target.value));

  const favorite = async () => {
    const output = await api.createFavoriteLocation({
      lat: currentLat,
      lon: currentLon,
    });
    if (output.error) return alert("Error to favorite location.");
    context.addFavoriteLocation({
      lat: currentLat,
      lon: currentLon,
      id: output.data.favoriteLocationId,
    });
    setIsFavorited(true);
  };

  const checkIfIsFavorited = () => {
    const location = context.favoriteLocations.find(
      (location) => location.lat === currentLat && location.lon === currentLon
    );
    setIsFavorited(Boolean(location));
  };

  return (
    <div>
      <h1>Location Weather Information</h1>

      <div>
        <form action="" method="get" onSubmit={searchLocation}>
          <div>
            <label htmlFor="lat">Latitute</label>
            <input
              type="search"
              id="lat"
              value={currentLat}
              onChange={updateCurrentLat}
            />
          </div>

          <div>
            <label htmlFor="lon">Longitude</label>
            <input
              type="search"
              id="lon"
              value={currentLon}
              onChange={updateCurrentLon}
            />
          </div>

          <input type="submit" value="Search Location" />
        </form>
      </div>

      {Boolean(currentLocation) && (
        <div>
          {(isFavorited && <div>* Favorited</div>) || (
            <div onClick={favorite}>Favorite</div>
          )}

          <div>
            <span>Current UTC Time Unix</span>
            <strong>{currentLocation?.currentUTCTimeUnix}</strong>
          </div>

          <div>
            <span>Temperature (Celsius)</span>
            <strong>{currentLocation?.temperatureInCelsius}</strong>
          </div>

          <div>
            <span>Feels Like (Celsius)</span>
            <strong>{currentLocation?.feelsLikeInCelsius}</strong>
          </div>

          <div>
            <span>Pressure (hPa)</span>
            <strong>{currentLocation?.pressureInhPa}</strong>
          </div>

          <div>
            <span>Humidity (%)</span>
            <strong>{currentLocation?.humidityPercent}</strong>
          </div>

          <div>
            <span>Clouds (%)</span>
            <strong>{currentLocation?.cloudsPercent}</strong>
          </div>

          <div>
            <span>Wind Speed (Meter / Second)</span>
            <strong>{currentLocation?.windSpeedInMeterPerSecond}</strong>
          </div>
        </div>
      )}
    </div>
  );
};
