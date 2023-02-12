import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useStore } from '../../../state/storeHooks';
import { ArticlesViewer } from '../../ArticlesViewer/ArticlesViewer';
import { UserInfo } from '../../UserInfo/UserInfo';
import tabs, { getProfilePageTabs } from '../../../services/tabs';
import { useLocalization } from '../../../services/localizations/localization';
import { onLoad } from './onLoad';
import { onFollowToggle } from './onFollowToggle';
import { onTabChange } from './onTabChange';
import { onPageChange } from './onPageChange';

export function ProfilePage() {
  const navigate = useNavigate();
  const { username, favorites: favaritesParam } = useParams<{ username: string; favorites?: string | undefined }>();
  const favorites = !!favaritesParam && favaritesParam.endsWith('favorites');
  if (!username) {
    throw 'username is undefined';
  }

  useEffect(() => {
    onLoad(username, favorites, navigate);
  }, [username]);

  const { profile, submitting } = useStore(({ profile }) => profile);

  const { localization } = useLocalization();
  return (
    <div className='profile-page'>
      {profile.match({
        none: () => (
          <div className='article-preview' key={1}>
            {localization.profile.loading}
          </div>
        ),
        some: (profile) => (
          <UserInfo
            user={profile}
            disabled={submitting}
            onFollowToggle={onFollowToggle(profile, navigate)}
            onEditSettings={() => navigate('settings')}
          />
        ),
      })}

      <div className='container'>
        <div className='row'>
          <div className='col-xs-12 col-md-10 offset-md-1'>
            <ArticlesViewer
              toggleClassName='articles-toggle'
              tabs={[tabs.favoritedArticlesTab, tabs.myArticlesTab]}
              tabsTranslation={getProfilePageTabs()}
              selectedTab={favorites ? tabs.favoritedArticlesTab : tabs.myArticlesTab}
              onTabChange={onTabChange(username, navigate)}
              onPageChange={onPageChange(username, favorites)}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
