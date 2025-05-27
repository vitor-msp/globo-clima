import { useContext, useState } from "react";
import { Country, api } from "../services/api";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";

export const CountriesPage = () => {
  const [currentCioc, setCurrentCioc] = useState<string>("BRA");
  const [currentCountry, setCurrentCountry] = useState<Country | null>(null);

  const context = useContext(FavoriteCountriesContext);

  const searchCountry = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.getCountryDemographicInformation(currentCioc);
    if (output.error)
      return alert("Error to get country demographic information.");
    setCurrentCountry(output.data);
  };

  const updateCurrentCioc = (event: React.ChangeEvent<HTMLInputElement>) =>
    setCurrentCioc(event.target.value);

  const favorite = async () => {
    const output = await api.createFavoriteCountry(currentCioc);
    if (output.error) return alert("Error to favorite country.");
    context.addFavoriteCountry({
      cioc: currentCioc,
      id: output.data.favoriteCountryId,
    });
  };

  return (
    <div>
      <h1>Country Demographic Information</h1>

      <div>
        <form action="" method="get" onSubmit={searchCountry}>
          <input
            type="search"
            value={currentCioc}
            onChange={updateCurrentCioc}
          />
          <input type="submit" value="Search Country" />
        </form>
      </div>

      {Boolean(currentCountry) && (
        <div>
          <div onClick={favorite}>Favoritar</div>

          <div>
            <span>Common Name</span>
            <strong>{currentCountry?.commonName}</strong>
          </div>

          <div>
            <span>Official Name</span>
            <strong>{currentCountry?.officialName}</strong>
          </div>

          <div>
            <span>Cioc</span>
            <strong>{currentCountry?.cioc}</strong>
          </div>

          <div>
            <span>Population</span>
            <strong>{currentCountry?.population}</strong>
          </div>

          <div>
            <span>Languages</span>
            <strong>{currentCountry?.languages.join(", ")}</strong>
          </div>

          <div>
            <span>Currencies</span>
            {(currentCountry !== null && (
              <ul>
                {currentCountry.currencies.map((currency) => (
                  <li>
                    <div>
                      <span>Symbol</span>
                      <strong>{currency.symbol}</strong>
                    </div>

                    <div>
                      <span>Name</span>
                      <strong>{currency.name}</strong>
                    </div>
                  </li>
                ))}
              </ul>
            )) || <span>No data to show.</span>}
          </div>
        </div>
      )}
    </div>
  );
};
