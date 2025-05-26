import { useState } from "react";
import { getLocationWeatherInformation, Location } from "../services/api";

export const LocationsPage = () => {
  const [currentLat, setCurrentLat] = useState<number>(-19.9191248);
  const [currentLon, setCurrentLon] = useState<number>(-43.9386291);
  const [currentLocation, setCurrentLocation] = useState<Location | null>(null);

  const searchLocation = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await getLocationWeatherInformation(currentLat, currentLon);
    if (output.error)
      return alert("Error to get location weather information.");
    setCurrentLocation(output.data);
  };

  const updateCurrentLat = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentLat(Number(event.target.value));

  const updateCurrentLon = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentLon(Number(event.target.value));

  return (
    <div>
      <h1>Location Weather Information</h1>

      <div>
        <form action="" method="get" onSubmit={searchLocation}>
          <input type="search" value={currentLat} onChange={updateCurrentLat} />
          <input type="search" value={currentLon} onChange={updateCurrentLon} />
          <input type="submit" value="Search Location" />
        </form>
      </div>

      {Boolean(currentLocation) && (
        <div>
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
