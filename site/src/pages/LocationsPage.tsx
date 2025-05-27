import { useState } from "react";
import { Location } from "../services/api";
import { LocationsSearch } from "../components/LocationsSearch";
import { LocationsInfo } from "../components/LocationsInfo";

export const LocationsPage = () => {
  const [currentLat, setCurrentLat] = useState<number>(-19.92);
  const [currentLon, setCurrentLon] = useState<number>(-43.94);
  const [currentLocation, setCurrentLocation] = useState<Location | null>(null);
  const [isFavorited, setIsFavorited] = useState<boolean>(false);

  return (
    <div>
      <h1>Location Weather Information</h1>

      <div>
        <LocationsSearch
          currentLat={currentLat}
          currentLon={currentLon}
          setCurrentLat={setCurrentLat}
          setCurrentLon={setCurrentLon}
          setIsFavorited={setIsFavorited}
          setCurrentLocation={setCurrentLocation}
        />
      </div>

      <LocationsInfo
        currentLat={currentLat}
        currentLocation={currentLocation}
        currentLon={currentLon}
        isFavorited={isFavorited}
        setIsFavorited={setIsFavorited}
      />
    </div>
  );
};
