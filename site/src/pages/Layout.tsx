import { Navbar } from "../components/Navbar";
import { useContext, useEffect } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { api } from "../services/api";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

export type LayoutProps = {
  child: any;
};

export const Layout: React.FC<LayoutProps> = ({ child }) => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

  const loadFavoriteCountries = async () => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.getFavoriteCountries(loginContext.accessToken!);
    if (output.error) return alert("Error to get favorite countries.");
    favoriteCountriesContext.setFavoriteCountries(output.data);
  };

  const loadFavoriteLocations = async () => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.getFavoriteLocations(loginContext.accessToken!);
    if (output.error) return alert("Error to get favorite locations.");
    favoriteLocationsContext.setFavoriteLocations(output.data);
  };

  useEffect(() => {
    (async () => {
      if (!Boolean(loginContext.accessToken)) return;
      await Promise.all([loadFavoriteCountries(), loadFavoriteLocations()]);
    })();
  }, [loginContext.accessToken]);

  return (
    <div>
      <Navbar />
      <main>{child}</main>
    </div>
  );
};
