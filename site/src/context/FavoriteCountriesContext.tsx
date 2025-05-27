import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteCountry } from "../services/api";

export type FavoriteCountriesContextType = {
  setFavoriteCountries: (favoriteCountries: FavoriteCountry[]) => void;
  favoriteCountries: FavoriteCountry[];
  addFavoriteCountry: (country: FavoriteCountry) => void;
  removeFavoriteCountry: (countryId: string) => void;
};

const defaultContext: FavoriteCountriesContextType = {
  setFavoriteCountries: () => {},
  favoriteCountries: [],
  addFavoriteCountry: () => {},
  removeFavoriteCountry: () => {},
};

export const FavoriteCountriesContext =
  createContext<FavoriteCountriesContextType>(defaultContext);

export const FavoriteCountriesProvider = ({ children }: PropsWithChildren) => {
  const [favoriteCountries, setFavoriteCountries] = useState<FavoriteCountry[]>(
    []
  );

  const addFavoriteCountry = (country: FavoriteCountry) =>
    favoriteCountries.push(country);

  const removeFavoriteCountry = (countryId: string) =>
    setFavoriteCountries(
      favoriteCountries.filter((country) => country.id !== countryId)
    );

  return (
    <FavoriteCountriesContext.Provider
      value={{
        setFavoriteCountries,
        favoriteCountries,
        addFavoriteCountry,
        removeFavoriteCountry,
      }}
    >
      {children}
    </FavoriteCountriesContext.Provider>
  );
};
