import { NavigateFunction } from 'react-router-dom';
import { getArticle, getArticleComments } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { Article } from '../../../types/article';
import { initializeArticlePage, loadArticle, loadComments } from './ArticlePage.slice';

export async function onLoad(slug: string, navigate: NavigateFunction) {
  store.dispatch(initializeArticlePage());
  let article: Article | undefined = undefined;
  try {
    article = await getArticle(slug);
    store.dispatch(loadArticle(article));
    const comments = await getArticleComments(slug);
    store.dispatch(loadComments(comments));
  } catch (e) {
    navigate('/');
  }
}
