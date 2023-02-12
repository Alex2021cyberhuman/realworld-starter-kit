import { getArticles } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';

export async function getArticlesByType(username: string, favorites: boolean) {
  const { currentPage } = store.getState().articleViewer;
  return await getArticles({
    [favorites ? 'favorited' : 'author']: username,
    offset: (currentPage - 1) * 10,
  });
}
