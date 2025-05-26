import { useEffect, useState } from "react";
import { FavoriteCountry, getFavoriteCountries } from "../services/api";

export const FavoriteCountriesPage = () => {
  const [favoriteCountries, setFavoriteCountries] = useState<FavoriteCountry[]>(
    []
  );

  useEffect(() => {
    (async () => {
      const output = await getFavoriteCountries();
      if (output.error) return alert("Error to get favorite countries.");
      setFavoriteCountries(output.data);
    })();
  }, []);

  return (
    <div>
      <h1>Favorite Countries</h1>

      <ul>
        {favoriteCountries.map((country) => (
          <li>
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
