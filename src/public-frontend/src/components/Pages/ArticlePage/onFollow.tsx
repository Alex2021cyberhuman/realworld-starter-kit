import { NavigateFunction } from 'react-router-dom';
import { followUser, unfollowUser } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { startSubmittingFollow, updateAuthor } from './ArticlePage.slice';

export async function onFollow(username: string, following: boolean, navigate: NavigateFunction) {
  if (store.getState().app.user.isNone()) {
    navigate('/register');
    return;
  }

  store.dispatch(startSubmittingFollow());

  const author = await (following ? unfollowUser : followUser)(username);
  store.dispatch(updateAuthor(author));
}
