import { useEffect, useState } from "react";
import { FavoriteLocation, getFavoriteLocations } from "../services/api";

export const FavoriteLocationsPage = () => {
  const [favoriteLocations, setFavoriteLocations] = useState<
    FavoriteLocation[]
  >([]);

  useEffect(() => {
    (async () => {
      const output = await getFavoriteLocations();
      if (output.error) return alert("Error to get favorite locations.");
      setFavoriteLocations(output.data);
    })();
  }, []);

  return (
    <div>
      <h1>Favorite Locations</h1>

      <ul>
        {favoriteLocations.map((location) => (
          <li>
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
