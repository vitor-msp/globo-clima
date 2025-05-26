import { Navbar } from "../components/Navbar";
import { useContext, useEffect } from "react";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { getFavoriteCountries, getFavoriteLocations } from "../services/api";

export type LayoutProps = {
  child: any;
};

export const Layout: React.FC<LayoutProps> = ({ child }) => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);

  const loadFavoriteCountries = async () => {
    const output = await getFavoriteCountries();
    if (output.error) return alert("Error to get favorite countries.");
    favoriteCountriesContext.setFavoriteCountries(output.data);
  };

  const loadFavoriteLocations = async () => {
    const output = await getFavoriteLocations();
    if (output.error) return alert("Error to get favorite locations.");
    favoriteLocationsContext.setFavoriteLocations(output.data);
  };

  useEffect(() => {
    (async () => {
      await Promise.all([loadFavoriteCountries(), loadFavoriteLocations()]);
    })();
  }, []);

  return (
    <div>
      <Navbar />
      <main>{child}</main>
    </div>
  );
};
