import { Option } from '@hqoss/monads';
import { Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Article } from '../../../types/article';
import { User } from '../../../types/user';
import { CommentSectionState } from './ArticlePage.slice';
import { formatString, useLocalization } from '../../../services/localizations/localization';
import { ArticleComment } from './ArticleComment';
import { CommentForm } from './CommentForm';

export function CommentSection({
  user,
  article,
  commentSection: { submittingComment, commentBody, comments },
}: {
  user: Option<User>;
  article: Article;
  commentSection: CommentSectionState;
}) {
  const { localization } = useLocalization();
  return (
    <div className='row'>
      <div className='col-xs-12 col-md-8 offset-md-2'>
        {user.match({
          none: () => (
            <p style={{ display: 'inherit' }}>
              {formatString(
                localization.comment.info,
                <Link to='/login'>{localization.comment.login}</Link>,
                <Link to='/register'>{localization.comment.register}</Link>
              )}
            </p>
          ),
          some: (user) => (
            <CommentForm
              user={user}
              slug={article.slug}
              submittingComment={submittingComment}
              commentBody={commentBody}
            />
          ),
        })}

        {comments.match({
          none: () => <div>{localization.comment.loading}</div>,
          some: (comments) => (
            <Fragment>
              {comments.map((comment, index) => (
                <ArticleComment key={comment.id} comment={comment} slug={article.slug} user={user} index={index} />
              ))}
            </Fragment>
          ),
        })}
      </div>
    </div>
  );
}
