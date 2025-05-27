import { useContext } from "react";
import { api, Location } from "../services/api";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

type LocationsInfoProps = {
  setIsFavorited: React.Dispatch<React.SetStateAction<boolean>>;
  currentLat: number;
  currentLon: number;
  currentLocation: Location | null;
  isFavorited: boolean;
};

export const LocationsInfo: React.FC<LocationsInfoProps> = ({
  setIsFavorited,
  currentLat,
  currentLon,
  currentLocation,
  isFavorited,
}) => {
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

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

  return (
    (Boolean(currentLocation) && (
      <div className="info">
        <div className="favorite-div">
          {(isFavorited && <span className="favorited">Favorited</span>) || (
            <span onClick={favorite} className="favorite">
              Favorite
            </span>
          )}
        </div>

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
    )) || <></>
  );
};
