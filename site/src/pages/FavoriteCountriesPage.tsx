import { useContext } from "react";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { FavoriteCountryItem } from "../components/FavoriteCountryItem";

export const FavoriteCountriesPage = () => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);

  return (
    <div>
      <h1>Favorite Countries</h1>

      <ul>
        {favoriteCountriesContext.favoriteCountries.map((country) => (
          <FavoriteCountryItem country={country} key={country.id} />
        ))}
      </ul>
    </div>
  );
};
