import { PropsWithChildren, createContext, useState } from "react";
import { FavoriteLocation } from "../services/api";

export type FavoriteLocationsContextType = {
  setFavoriteLocations: (favoriteLocations: FavoriteLocation[]) => void;
  favoriteLocations: FavoriteLocation[];
  addFavoriteLocation: (location: FavoriteLocation) => void;
  removeFavoriteLocation: (locationId: string) => void;
};

const defaultContext: FavoriteLocationsContextType = {
  setFavoriteLocations: () => {},
  favoriteLocations: [],
  addFavoriteLocation: () => {},
  removeFavoriteLocation: () => {},
};

export const FavoriteLocationsContext =
  createContext<FavoriteLocationsContextType>(defaultContext);

export const FavoriteLocationsProvider = ({ children }: PropsWithChildren) => {
  const [favoriteLocations, setFavoriteLocations] = useState<
    FavoriteLocation[]
  >([]);

  const addFavoriteLocation = (location: FavoriteLocation) =>
    favoriteLocations.push(location);

  const removeFavoriteLocation = (locationId: string) =>
    setFavoriteLocations(
      favoriteLocations.filter((location) => location.id !== locationId)
    );

  return (
    <FavoriteLocationsContext.Provider
      value={{
        setFavoriteLocations,
        favoriteLocations,
        addFavoriteLocation,
        removeFavoriteLocation,
      }}
    >
      {children}
    </FavoriteLocationsContext.Provider>
  );
};
