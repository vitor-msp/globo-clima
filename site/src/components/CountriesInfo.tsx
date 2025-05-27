import { useContext } from "react";
import { api, Country } from "../services/api";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

type CountriesInfoProps = {
  setIsFavorited: React.Dispatch<React.SetStateAction<boolean>>;
  currentCioc: string;
  currentCountry: Country | null;
  isFavorited: boolean;
};

export const CountriesInfo: React.FC<CountriesInfoProps> = ({
  setIsFavorited,
  currentCioc,
  currentCountry,
  isFavorited,
}) => {
  const favoriteCountriesContext = useContext(FavoriteCountriesContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

  const favorite = async () => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.createFavoriteCountry(
      { cioc: currentCioc },
      loginContext.accessToken!
    );
    if (output.error) return alert("Error to favorite country.");
    favoriteCountriesContext.addFavoriteCountry({
      cioc: currentCioc,
      id: output.data.favoriteCountryId,
    });
    setIsFavorited(true);
  };

  return (
    <div>
      {Boolean(currentCountry) && (
        <div>
          {(isFavorited && <div>* Favorited</div>) || (
            <div onClick={favorite}>Favorite</div>
          )}

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
