import { NavigateFunction } from 'react-router-dom';
import { deleteArticle } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { startDeletingArticle } from './ArticlePage.slice';

export async function onDeleteArticle(slug: string, navigate: NavigateFunction) {
  store.dispatch(startDeletingArticle());
  await deleteArticle(slug);
  navigate('/');
}
