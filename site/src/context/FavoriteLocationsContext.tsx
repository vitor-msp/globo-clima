import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteLocation } from "../services/api";

export type FavoriteLocationsContextType = {
  setFavoriteLocations: (favoriteLocations: FavoriteLocation[]) => void;
  favoriteLocations: FavoriteLocation[];
};

const defaultContext: FavoriteLocationsContextType = {
  setFavoriteLocations: () => {},
  favoriteLocations: [],
};

export const FavoriteLocationsContext =
  createContext<FavoriteLocationsContextType>(defaultContext);

export const FavoriteLocationsProvider = ({ children }: PropsWithChildren) => {
  const [favoriteLocations, setFavoriteLocations] = useState<
    FavoriteLocation[]
  >([]);

  return (
    <FavoriteLocationsContext.Provider
      value={{
        setFavoriteLocations,
        favoriteLocations,
      }}
    >
      {children}
    </FavoriteLocationsContext.Provider>
  );
};
