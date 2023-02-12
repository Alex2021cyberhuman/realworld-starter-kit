import axios from 'axios';
import { date, Decoder, nullable, object, optional, string } from 'decoders';
import { loadUser } from '../components/App/App.slice';
import { store } from '../state/store';
import jwtDecode from 'jwt-decode';

export interface PublicUser {
  username: string;
  bio: string | null;
  image: string | null;
}

export interface User extends PublicUser {
  email: string;
  token: string;
  accessTokenExpireTime?: Date | undefined;
}

export const userDecoder: Decoder<User> = object({
  email: string,
  token: string,
  username: string,
  accessTokenExpireTime: optional(date),
  bio: nullable(string),
  image: nullable(string),
});

export interface UserSettings extends PublicUser {
  email: string;
  password: string | null;
}

export interface UserForRegistration {
  username: string;
  email: string;
  password: string;
}

interface TokenData {
  iat: number;
  jti: string;
  sub: string;
  exp: number;
}

export function getAccessTokenExpire(token?: string | undefined | null) {
  if (!token) {
    return undefined;
  }

  try {
    const decodedTokenData = jwtDecode<TokenData>(token);
    const expireAtSeconds = decodedTokenData.exp;
    return new Date(expireAtSeconds * 1000);
  } catch (e) {
    return undefined;
  }
}

export function loadToken(token: string) {
  axios.defaults.headers.common.Authorization = `Token ${token}`;
}

export function loadUserIntoApp(user: User) {
  localStorage.setItem('token', user.token);
  loadToken(user.token);
  user.accessTokenExpireTime = getAccessTokenExpire(user.token);
  store.dispatch(loadUser(user));
}
