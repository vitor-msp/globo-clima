import { useContext } from "react";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { api, FavoriteCountry } from "../services/api";
import { useNavigate } from "react-router-dom";
import { LoginContext } from "../context/LoginContext";

export const FavoriteCountryItem: React.FC<{ country: FavoriteCountry }> = ({
  country,
}) => {
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
    <li key={country.id}>
      <div onClick={() => unfavorite(country.id)} className="unfavorite">
        <span>X</span>
      </div>

      <div>
        <span>Country IOC</span>
        <strong>{country.cioc}</strong>
      </div>
    </li>
  );
};
