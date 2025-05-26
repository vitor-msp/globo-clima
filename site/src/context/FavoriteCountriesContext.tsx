import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteCountry } from "../services/api";

export type FavoriteCountriesContextType = {
  setFavoriteCountries: (favoriteCountries: FavoriteCountry[]) => void;
  favoriteCountries: FavoriteCountry[];
};

const defaultContext: FavoriteCountriesContextType = {
  setFavoriteCountries: () => {},
  favoriteCountries: [],
};

export const FavoriteCountriesContext =
  createContext<FavoriteCountriesContextType>(defaultContext);

export const FavoriteCountriesProvider = ({ children }: PropsWithChildren) => {
  const [favoriteCountries, setFavoriteCountries] = useState<FavoriteCountry[]>(
    []
  );

  return (
    <FavoriteCountriesContext.Provider
      value={{
        setFavoriteCountries,
        favoriteCountries,
      }}
    >
      {children}
    </FavoriteCountriesContext.Provider>
  );
};
