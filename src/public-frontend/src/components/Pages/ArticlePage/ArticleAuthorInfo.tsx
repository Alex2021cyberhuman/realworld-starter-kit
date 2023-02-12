import { Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Article } from '../../../types/article';
import { getLocalDate } from '../../../services/localizations/localization';
import { SmallAvatar } from '../../Avatars/SmallAvatar';

export function ArticleAuthorInfo({
  article: {
    author: { username, image },
    createdAt,
  },
}: {
  article: Article;
}) {
  return (
    <Fragment>
      <Link to={`/profile/${username}`}>
        <SmallAvatar image={image} />
      </Link>
      <div className='info'>
        <Link className='author' to={`/profile/${username}`}>
          {username}
        </Link>
        <span className='date'>{getLocalDate(createdAt)}</span>
      </div>
    </Fragment>
  );
}
