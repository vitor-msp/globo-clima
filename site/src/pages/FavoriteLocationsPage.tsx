import { useContext } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { FavoriteLocationItem } from "../components/FavoriteLocationItem";

export const FavoriteLocationsPage = () => {
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);

  return (
    <div>
      <h1>Favorite Locations</h1>

      <ul>
        {favoriteLocationsContext.favoriteLocations.map((location) => (
          <FavoriteLocationItem location={location} key={location.id} />
        ))}
      </ul>
    </div>
  );
};
