import { useContext } from "react";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { api } from "../services/api";
import { useNavigate } from "react-router-dom";
import { LoginContext } from "../context/LoginContext";

export const FavoriteCountriesPage = () => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

  const unfavorite = async (countryId: string) => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.removeFavoriteCountry(
      countryId,
      loginContext.accessToken!
    );
    if (output.error) return alert("Error to unfavorite country.");
    favoriteCountriesContext.removeFavoriteCountry(countryId);
  };

  return (
    <div>
      <h1>Favorite Countries</h1>

      <ul>
        {favoriteCountriesContext.favoriteCountries.map((country) => (
          <li key={country.id}>
            <div onClick={() => unfavorite(country.id)}>Unfavorite</div>

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
