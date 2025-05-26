import { useContext } from "react";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";

export const FavoriteCountriesPage = () => {
  const context = useContext(FavoriteCountriesContext);

  return (
    <div>
      <h1>Favorite Countries</h1>

      <ul>
        {context.favoriteCountries.map((country) => (
          <li key={country.id}>
            <div>
              <span>Cioc</span>
              <strong>{country.cioc}</strong>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};
