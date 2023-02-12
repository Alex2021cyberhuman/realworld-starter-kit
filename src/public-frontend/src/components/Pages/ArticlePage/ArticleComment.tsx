import { Option } from '@hqoss/monads';
import { format } from 'date-fns';
import React from 'react';
import { Link } from 'react-router-dom';
import { Comment } from '../../../types/comment';
import { User } from '../../../types/user';
import { formatString, useLocalization } from '../../../services/localizations/localization';
import { SmallAvatar } from '../../Avatars/SmallAvatar';
import { onDeleteComment } from './onDeleteComment';

export function ArticleComment({
  comment: {
    id,
    body,
    createdAt,
    author: { username, image },
  },
  slug,
  index,
  user,
}: {
  comment: Comment;
  slug: string;
  index: number;
  user: Option<User>;
}) {
  const { localization } = useLocalization();
  return (
    <div className='card'>
      <div className='card-block'>
        <p className='card-text'>{body}</p>
      </div>
      <div className='card-footer'>
        <Link className='comment-author' to={`/profile/${username}`}>
          <SmallAvatar image={image} className='comment-author-img' />
        </Link>
        &nbsp;
        <Link className='comment-author' to={`/profile/${username}`}>
          {username}
        </Link>
        <span className='date-posted'>{format(createdAt, 'PP')}</span>
        {user.isSome() && user.unwrap().username === username && (
          <span className='mod-options'>
            <i
              className='ion-trash-a'
              aria-label={formatString(localization.comment.delete, index + 1).toString()}
              onClick={() => onDeleteComment(slug, id)}
            >
              <wbr />
            </i>
          </span>
        )}
      </div>
    </div>
  );
}
