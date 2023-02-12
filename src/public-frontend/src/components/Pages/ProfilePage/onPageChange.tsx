import { store } from '../../../state/store';
import { changePage, loadArticles } from '../../ArticlesViewer/ArticlesViewer.slice';
import { getArticlesByType } from './getArticlesByType';

export function onPageChange(username: string, favorited: boolean): (index: number) => void {
  return async (index) => {
    store.dispatch(changePage(index));
    store.dispatch(loadArticles(await getArticlesByType(username, favorited)));
  };
}
