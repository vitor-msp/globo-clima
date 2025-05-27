import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteLocation } from "../services/api";

export type FavoriteLocationsContextType = {
  setFavoriteLocations: (favoriteLocations: FavoriteLocation[]) => void;
  favoriteLocations: FavoriteLocation[];
  addFavoriteLocation: (location: FavoriteLocation) => void;
};

const defaultContext: FavoriteLocationsContextType = {
  setFavoriteLocations: () => {},
  favoriteLocations: [],
  addFavoriteLocation: () => {},
};

export const FavoriteLocationsContext =
  createContext<FavoriteLocationsContextType>(defaultContext);

export const FavoriteLocationsProvider = ({ children }: PropsWithChildren) => {
  const [favoriteLocations, setFavoriteLocations] = useState<
    FavoriteLocation[]
  >([]);

  const addFavoriteLocation = (location: FavoriteLocation) =>
    favoriteLocations.push(location);

  return (
    <FavoriteLocationsContext.Provider
      value={{
        setFavoriteLocations,
        favoriteLocations,
        addFavoriteLocation,
      }}
    >
      {children}
    </FavoriteLocationsContext.Provider>
  );
};
