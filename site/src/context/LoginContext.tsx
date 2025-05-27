import { PropsWithChildren, createContext, useEffect, useState } from "react";

export type LoginContextType = {
  login: (accessToken: string) => void;
  logout: () => void;
  isLogged: boolean;
  accessToken: string | null;
};

const defaultContext: LoginContextType = {
  login: () => {},
  logout: () => {},
  isLogged: false,
  accessToken: null,
};

export const LoginContext = createContext<LoginContextType>(defaultContext);

export const LoginProvider = ({ children }: PropsWithChildren) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [isLogged, setIsLogged] = useState<boolean>(false);

  useEffect(() => {
    const accessToken = localStorage.getItem("globo-clima-access-token");
    if (!Boolean(accessToken)) return;
    login(accessToken!);
  }, []);

  const login = (accessToken: string) => {
    setAccessToken(accessToken);
    localStorage.setItem("globo-clima-access-token", accessToken);
    setIsLogged(true);
  };

  const logout = () => {
    setAccessToken(null);
    localStorage.removeItem("globo-clima-access-token");
    setIsLogged(false);
  };

  return (
    <LoginContext.Provider value={{ login, logout, isLogged, accessToken }}>
      {children}
    </LoginContext.Provider>
  );
};
