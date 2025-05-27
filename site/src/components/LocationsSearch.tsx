import { useContext } from "react";
import { api, Location } from "../services/api";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";

type CountriesSearchProps = {
  setIsFavorited: React.Dispatch<React.SetStateAction<boolean>>;
  currentLat: number;
  currentLon: number;
  setCurrentLat: React.Dispatch<React.SetStateAction<number>>;
  setCurrentLon: React.Dispatch<React.SetStateAction<number>>;
  setCurrentLocation: React.Dispatch<React.SetStateAction<Location | null>>;
};

export const LocationsSearch: React.FC<CountriesSearchProps> = ({
  setIsFavorited,
  currentLat,
  currentLon,
  setCurrentLat,
  setCurrentLon,
  setCurrentLocation,
}) => {
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);

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

  const checkIfIsFavorited = () => {
    const location = favoriteLocationsContext.favoriteLocations.find(
      (location) => location.lat === currentLat && location.lon === currentLon
    );
    setIsFavorited(Boolean(location));
  };

  return (
    <form action="" method="get" onSubmit={searchLocation}>
      <div>
        <label htmlFor="lat">Latitute</label>
        <input
          type="number"
          id="lat"
          value={currentLat}
          onChange={updateCurrentLat}
          min={-90.0}
          max={90.0}
          step={0.01}
        />
      </div>

      <div>
        <label htmlFor="lon">Longitude</label>
        <input
          type="number"
          id="lon"
          value={currentLon}
          onChange={updateCurrentLon}
          min={-180.0}
          max={180.0}
          step={0.01}
        />
      </div>

      <input type="submit" value="Search Location" />
    </form>
  );
};
