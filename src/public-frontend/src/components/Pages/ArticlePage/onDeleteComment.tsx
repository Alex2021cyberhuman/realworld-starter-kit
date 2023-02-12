import { deleteComment, getArticleComments } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { loadComments } from './ArticlePage.slice';

export async function onDeleteComment(slug: string, id: string) {
  await deleteComment(slug, id);
  store.dispatch(loadComments(await getArticleComments(slug)));
}
