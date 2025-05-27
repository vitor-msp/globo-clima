import { useContext } from "react";
import { api, Country } from "../services/api";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";

type CountriesSearchProps = {
  setCurrentCountry: React.Dispatch<React.SetStateAction<Country | null>>;
  setCurrentCioc: React.Dispatch<React.SetStateAction<string>>;
  setIsFavorited: React.Dispatch<React.SetStateAction<boolean>>;
  currentCioc: string;
};

export const CountriesSearch: React.FC<CountriesSearchProps> = ({
  setCurrentCountry,
  setCurrentCioc,
  setIsFavorited,
  currentCioc,
}) => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);

  const searchCountry = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.getCountryDemographicInformation(currentCioc);
    if (output.error)
      return alert("Error to get country demographic information.");
    setCurrentCountry(output.data);
    checkIfIsFavorited();
  };

  const updateCurrentCioc = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentCioc(event.target.value);

  const checkIfIsFavorited = () => {
    const country = favoriteCountriesContext.favoriteCountries.find(
      (country) => country.cioc === currentCioc
    );
    setIsFavorited(Boolean(country));
  };

  return (
    <form action="" method="get" onSubmit={searchCountry}>
      <div>
        <label htmlFor="cioc">Cioc</label>
        <input
          type="search"
          id="cioc"
          value={currentCioc}
          onChange={updateCurrentCioc}
        />
      </div>

      <input type="submit" value="Search Country" />
    </form>
  );
};
