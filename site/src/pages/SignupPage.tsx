import { useState } from "react";
import { api, SignupInput } from "../services/api";
import { useNavigate } from "react-router-dom";

export const SignupPage = () => {
  const [input, setInput] = useState<SignupInput>({
    name: "",
    username: "",
    password: "",
    passwordConfirmation: "",
  });

  const navigate = useNavigate();

  const signup = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const output = await api.signup(input);
    if (output.error) return alert("Error to sign up.");
    navigate("/login");
  };

  const updateField = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInput({ ...input, [event.target.name]: event.target.value });
  };

  return (
    <div>
      <h1>Signup Page</h1>

      <div>
        <form action="" onSubmit={signup}>
          <div>
            <label htmlFor="name">Name</label>
            <input
              type="text"
              id="name"
              name="name"
              onChange={updateField}
              value={input.name}
            />
          </div>

          <div>
            <label htmlFor="username">Unique username</label>
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

          <div>
            <label htmlFor="passwordConfirmation">Password confirmation</label>
            <input
              type="password"
              id="passwordConfirmation"
              name="passwordConfirmation"
              onChange={updateField}
              value={input.passwordConfirmation}
            />
          </div>

          <input type="submit" value="Sign Up" />
        </form>
      </div>
    </div>
  );
};
