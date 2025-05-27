import { useContext } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { api, FavoriteLocation } from "../services/api";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

export const FavoriteLocationItem: React.FC<{
  location: FavoriteLocation;
}> = ({ location }) => {
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

  const unfavorite = async (locationId: string) => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.removeFavoriteLocation(
      locationId,
      loginContext.accessToken!
    );
    if (output.error) return alert("Error to unfavorite location.");
    favoriteLocationsContext.removeFavoriteLocation(locationId);
  };

  return (
    <li key={location.id}>
      <div onClick={() => unfavorite(location.id)} className="unfavorite">
        <span>X</span>
      </div>

      <div>
        <span>Latitude</span>
        <strong>{location.lat}</strong>
      </div>

      <div>
        <span>Longitude</span>
        <strong>{location.lon}</strong>
      </div>
    </li>
  );
};
