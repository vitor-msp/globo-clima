import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteCountry } from "../services/api";

export type FavoriteCountriesContextType = {
  setFavoriteCountries: (favoriteCountries: FavoriteCountry[]) => void;
  favoriteCountries: FavoriteCountry[];
  addFavoriteCountry: (country: FavoriteCountry) => void;
};

const defaultContext: FavoriteCountriesContextType = {
  setFavoriteCountries: () => {},
  favoriteCountries: [],
  addFavoriteCountry: () => {},
};

export const FavoriteCountriesContext =
  createContext<FavoriteCountriesContextType>(defaultContext);

export const FavoriteCountriesProvider = ({ children }: PropsWithChildren) => {
  const [favoriteCountries, setFavoriteCountries] = useState<FavoriteCountry[]>(
    []
  );

  const addFavoriteCountry = (country: FavoriteCountry) =>
    favoriteCountries.push(country);

  return (
    <FavoriteCountriesContext.Provider
      value={{
        setFavoriteCountries,
        favoriteCountries,
        addFavoriteCountry,
      }}
    >
      {children}
    </FavoriteCountriesContext.Provider>
  );
};
