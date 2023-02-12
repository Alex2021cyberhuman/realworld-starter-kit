import { NavigateFunction } from 'react-router-dom';
import { store } from '../../../state/store';
import { loadArticles, startLoadingArticles } from '../../ArticlesViewer/ArticlesViewer.slice';
import tabs from '../../../services/tabs';
import { getArticlesByType } from './getArticlesByType';

export function onTabChange(username: string, navigate: NavigateFunction): (page: string) => void {
  return async (page) => {
    const favorited = page === tabs.favoritedArticlesTab;
    const path = `/profile/${username}${!favorited ? '' : '/favorites'}`;
    navigate(path);
    store.dispatch(startLoadingArticles());
    const articles = await getArticlesByType(username, favorited);
    store.dispatch(loadArticles(articles));
  };
}
