import { useContext } from "react";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { api } from "../services/api";

export const FavoriteCountriesPage = () => {
  const context = useContext(FavoriteCountriesContext);

  const unfavorite = async (countryId: string) => {
    const output = await api.removeFavoriteCountry(countryId);
    if (output.error) return alert("Error to unfavorite country.");
    context.removeFavoriteCountry(countryId);
  };

  return (
    <div>
      <h1>Favorite Countries</h1>

      <ul>
        {context.favoriteCountries.map((country) => (
          <li key={country.id}>
            <div onClick={() => unfavorite(country.id)}>Desfavoritar</div>

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
