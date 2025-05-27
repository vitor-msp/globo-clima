import { useContext, useState } from "react";
import { api, Location } from "../services/api";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

export const LocationsPage = () => {
  const [currentLat, setCurrentLat] = useState<number>(-20);
  const [currentLon, setCurrentLon] = useState<number>(-44);
  const [currentLocation, setCurrentLocation] = useState<Location | null>(null);
  const [isFavorited, setIsFavorited] = useState<boolean>(false);

  const favoriteLocationsContext = useContext(FavoriteLocationsContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

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
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.createFavoriteLocation(
      {
        lat: currentLat,
        lon: currentLon,
      },
      loginContext.accessToken!
    );
    if (output.error) return alert("Error to favorite location.");
    favoriteLocationsContext.addFavoriteLocation({
      lat: currentLat,
      lon: currentLon,
      id: output.data.favoriteLocationId,
    });
    setIsFavorited(true);
  };

  const checkIfIsFavorited = () => {
    const location = favoriteLocationsContext.favoriteLocations.find(
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
              type="number"
              id="lat"
              value={currentLat}
              onChange={updateCurrentLat}
              min={-90}
              max={90}
              step={1}
            />
          </div>

          <div>
            <label htmlFor="lon">Longitude</label>
            <input
              type="number"
              id="lon"
              value={currentLon}
              onChange={updateCurrentLon}
              min={-180}
              max={180}
              step={1}
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
