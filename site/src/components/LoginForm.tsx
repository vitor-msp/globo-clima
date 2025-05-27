import { useContext, useEffect, useState } from "react";
import { api, LoginInput } from "../services/api";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";
import { FavoriteCountriesContext } from "../context/FavoriteCountriesContext";
import { FavoriteLocationsContext } from "../context/FavoriteLocationsContext";

export const LoginForm = () => {
  const [input, setInput] = useState<LoginInput>({
    username: "",
    password: "",
  });

  const favoriteCountriesContext = useContext(FavoriteCountriesContext);
  const favoriteLocationsContext = useContext(FavoriteLocationsContext);
  const loginContext = useContext(LoginContext);

  const navigate = useNavigate();

  const login = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.login(input);
    if (output.error) return alert("Error to sign up.");
    loginContext.login(output.data.accessToken);
  };

  useEffect(() => {
    (async () => {
      if (!Boolean(loginContext.accessToken)) return;
      await Promise.all([loadFavoriteCountries(), loadFavoriteLocations()]);
      navigate("/countries");
    })();
  }, [loginContext.accessToken]);

  const updateField = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInput({ ...input, [event.target.name]: event.target.value });
  };

  const loadFavoriteCountries = async () => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.getFavoriteCountries(loginContext.accessToken!);
    if (output.error) return alert("Error to get favorite countries.");
    favoriteCountriesContext.setFavoriteCountries(output.data);
  };

  const loadFavoriteLocations = async () => {
    if (!Boolean(loginContext.accessToken)) {
      alert("You must login.");
      return navigate("/login");
    }
    const output = await api.getFavoriteLocations(loginContext.accessToken!);
    if (output.error) return alert("Error to get favorite locations.");
    favoriteLocationsContext.setFavoriteLocations(output.data);
  };

  return (
    <form onSubmit={login}>
      <div>
        <label htmlFor="username">Username</label>
        <input
          type="text"
          id="username"
          name="username"
          onChange={updateField}
          value={input.username}
        />
      </div>

      <div>
        <label htmlFor="password">Password</label>
        <input
          type="password"
          id="password"
          name="password"
          onChange={updateField}
          value={input.password}
        />
      </div>

      <input type="submit" value="Login" />
    </form>
  );
};
