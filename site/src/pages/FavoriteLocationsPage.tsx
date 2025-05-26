import { useContext } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";

export const FavoriteLocationsPage = () => {
  const context = useContext(FavoriteLocationsContext);

  return (
    <div>
      <h1>Favorite Locations</h1>

      <ul>
        {context.favoriteLocations.map((location) => (
          <li key={location.id}>
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
