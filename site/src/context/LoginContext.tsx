import { PropsWithChildren, createContext, useEffect, useState } from "react";

export type LoginContextType = {
  setAccessToken: (accessToken: string | null) => void;
  accessToken: string | null;
};

const defaultContext: LoginContextType = {
  setAccessToken: () => {},
  accessToken: null,
};

export const LoginContext = createContext<LoginContextType>(defaultContext);

export const LoginProvider = ({ children }: PropsWithChildren) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);

  useEffect(() => {
    if (Boolean(accessToken)) {
      return localStorage.setItem("globo-clima-access-token", accessToken!);
    }
    localStorage.removeItem("globo-clima-access-token");
  }, [accessToken]);

  return (
    <LoginContext.Provider value={{ accessToken, setAccessToken }}>
      {children}
    </LoginContext.Provider>
  );
};
