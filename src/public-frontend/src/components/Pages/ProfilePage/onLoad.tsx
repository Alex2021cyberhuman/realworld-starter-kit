import { NavigateFunction } from 'react-router-dom';
import { getProfile } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { loadArticles, startLoadingArticles } from '../../ArticlesViewer/ArticlesViewer.slice';
import { initializeProfile, loadProfile } from './ProfilePage.slice';
import { getArticlesByType } from './getArticlesByType';

export async function onLoad(username: string, favorites: boolean, navigate: NavigateFunction) {
  store.dispatch(initializeProfile());
  store.dispatch(startLoadingArticles());
  const profile = await getProfile(username);
  try {
    store.dispatch(loadProfile(profile));
    const articles = await getArticlesByType(username, favorites);
    store.dispatch(loadArticles(articles));
  } catch (e) {
    navigate('/');
  }
}
