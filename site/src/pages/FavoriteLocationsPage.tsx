import { useContext } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { api } from "../services/api";

export const FavoriteLocationsPage = () => {
  const context = useContext(FavoriteLocationsContext);

  const unfavorite = async (locationId: string) => {
    const output = await api.removeFavoriteLocation(locationId);
    if (output.error) return alert("Error to unfavorite location.");
    context.removeFavoriteLocation(locationId);
  };

  return (
    <div>
      <h1>Favorite Locations</h1>

      <ul>
        {context.favoriteLocations.map((location) => (
          <li key={location.id}>
            <div onClick={() => unfavorite(location.id)}>Unfavorite</div>

            <div>
              <span>Latitude</span>
              <span>{location.lat}</span>
            </div>

            <div>
              <span>Longitue</span>
              <span>{location.lon}</span>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};
