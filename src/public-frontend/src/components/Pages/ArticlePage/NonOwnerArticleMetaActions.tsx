import { Fragment } from 'react';
import { useNavigate } from 'react-router-dom';
import { Article } from '../../../types/article';
import { classObjectToClassName } from '../../../types/style';
import { useLocalization } from '../../../services/localizations/localization';
import { onFavorite } from './onFavorite';
import { onFollow } from './onFollow';

export function NonOwnerArticleMetaActions({
  article: {
    slug,
    favoritesCount,
    favorited,
    author: { username, following },
  },
  submittingFavorite,
  submittingFollow,
}: {
  article: Article;
  submittingFavorite: boolean;
  submittingFollow: boolean;
}) {
  const { localization } = useLocalization();
  const navigate = useNavigate();
  return (
    <Fragment>
      <button
        className={classObjectToClassName({
          btn: true,
          'btn-sm': true,
          'btn-outline-secondary': !following,
          'btn-secondary': following,
        })}
        disabled={submittingFollow}
        onClick={() => onFollow(username, following, navigate)}
      >
        <i className='ion-plus-round'></i>
        &nbsp; {`${following ? localization.userInfo.unfollow : localization.userInfo.follow} ${username}`}
      </button>
      &nbsp;
      <button
        className={classObjectToClassName({
          btn: true,
          'btn-sm': true,
          'btn-outline-primary': !favorited,
          'btn-primary': favorited,
        })}
        disabled={submittingFavorite}
        onClick={() => onFavorite(slug, favorited, navigate)}
      >
        <i className='ion-heart'></i>
        &nbsp; {favorited ? localization.article.unfavorite : localization.article.favorite}
        <span className='counter'>({favoritesCount})</span>
      </button>
    </Fragment>
  );
}
