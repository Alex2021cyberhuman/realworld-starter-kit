import { NavigateFunction } from 'react-router-dom';
import { favoriteArticle, unfavoriteArticle } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { endSubmittingFavorite, startSubmittingFavorite } from './ArticlePage.slice';

export async function onFavorite(slug: string, favorited: boolean, navigate: NavigateFunction) {
  if (store.getState().app.user.isNone()) {
    navigate('/register');
    return;
  }

  store.dispatch(startSubmittingFavorite());

  await (favorited ? unfavoriteArticle : favoriteArticle)(slug);
  store.dispatch(endSubmittingFavorite({ favorited }));
}
