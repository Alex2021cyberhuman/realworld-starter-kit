import { NavigateFunction } from 'react-router-dom';
import { followUser, unfollowUser } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { Profile } from '../../../types/profile';
import { loadProfile, startSubmitting } from './ProfilePage.slice';

export function onFollowToggle(profile: Profile, navigate: NavigateFunction): () => void {
  return async () => {
    const { user } = store.getState().app;
    if (user.isNone()) {
      navigate('/register');
      return;
    }

    store.dispatch(startSubmitting());

    const newProfile = await (profile.following ? unfollowUser : followUser)(profile.username);
    store.dispatch(loadProfile(newProfile));
  };
}
