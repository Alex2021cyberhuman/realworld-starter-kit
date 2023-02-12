import { getUser } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { getAccessTokenExpire, loadToken, loadUserIntoApp } from '../../../types/user';
import { endLoad, loadLanguage, logout } from '../App.slice';

export async function load() {
  const token = localStorage.getItem('token');
  const now = new Date();
  store.dispatch(loadLanguage());
  const expire = getAccessTokenExpire(token);
  if (!token || !expire || expire < now) {
    store.dispatch(endLoad());
    return;
  } else {
    try {
      loadToken(token);
      const user = await getUser();
      loadUserIntoApp(user);
    } catch {
      store.dispatch(logout());
      store.dispatch(endLoad());
    }
  }
}
