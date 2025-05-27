import { useState } from "react";
import { Country } from "../services/api";
import { CountriesInfo } from "../components/CountriesInfo";
import { CountriesSearch } from "../components/CountriesSearch";

export const CountriesPage = () => {
  const [currentCioc, setCurrentCioc] = useState<string>("BRA");
  const [currentCountry, setCurrentCountry] = useState<Country | null>(null);
  const [isFavorited, setIsFavorited] = useState<boolean>(false);

  return (
    <div>
      <h1>Country Demographic Information</h1>

      <div>
        <CountriesSearch
          currentCioc={currentCioc}
          setCurrentCioc={setCurrentCioc}
          setCurrentCountry={setCurrentCountry}
          setIsFavorited={setIsFavorited}
        />
      </div>

      <CountriesInfo
        currentCioc={currentCioc}
        currentCountry={currentCountry}
        isFavorited={isFavorited}
        setIsFavorited={setIsFavorited}
      />
    </div>
  );
};
