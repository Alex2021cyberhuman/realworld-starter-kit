import { loadUserIntoApp } from '../../types/user';
import { logout } from '../../components/App/App.slice';
import { getUser } from './conduit';

export function refreshToken() {
  getUser()
    .then((user) => loadUserIntoApp(user))
    .catch(() => logout());
}
