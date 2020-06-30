export type User = {
  username: string;
  password: string;
};

export function defaultUser() {
  const defaultUser = {
    username: '',
    password: '',
  };
  return defaultUser;
}

export default User;
