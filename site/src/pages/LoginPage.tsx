import { useContext, useState } from "react";
import { api, LoginInput } from "../services/api";
import { LoginContext } from "../context/LoginContext";
import { useNavigate } from "react-router-dom";

export const LoginPage = () => {
  const [input, setInput] = useState<LoginInput>({
    username: "",
    password: "",
  });

  const context = useContext(LoginContext);

  const navigate = useNavigate();

  const login = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.login(input);
    if (output.error) return alert("Error to sign up.");
    context.setAccessToken(output.data.accessToken);
    navigate("/countries");
  };

  const updateField = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInput({ ...input, [event.target.name]: event.target.value });
  };

  return (
    <div>
      <h1>Login Page</h1>

      <div>
        <form action="" onSubmit={login}>
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
      </div>
    </div>
  );
};
