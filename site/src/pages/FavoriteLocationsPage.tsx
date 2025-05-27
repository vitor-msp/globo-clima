import { useContext } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { api } from "../services/api";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

export const FavoriteLocationsPage = () => {
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
    <div>
      <h1>Favorite Locations</h1>

      <ul>
        {favoriteLocationsContext.favoriteLocations.map((location) => (
          <li key={location.id}>
            <div onClick={() => unfavorite(location.id)}>Unfavorite</div>

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
