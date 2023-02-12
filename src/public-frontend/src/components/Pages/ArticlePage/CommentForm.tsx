import { User } from '../../../types/user';
import { useLocalization } from '../../../services/localizations/localization';
import { SmallAvatar } from '../../Avatars/SmallAvatar';
import { onPostComment } from './onPostComment';
import { onCommentChange } from './onCommentChange';

export function CommentForm({
  user: { image },
  commentBody,
  slug,
  submittingComment,
}: {
  user: User;
  commentBody: string;
  slug: string;
  submittingComment: boolean;
}) {
  const { localization } = useLocalization();
  return (
    <form className='card comment-form' onSubmit={onPostComment(slug, commentBody)}>
      <div className='card-block'>
        <textarea
          className='form-control'
          placeholder={localization.comment.write}
          rows={3}
          onChange={onCommentChange}
          value={commentBody}
        ></textarea>
      </div>
      <div className='card-footer'>
        <SmallAvatar image={image} className='comment-author-img' />
        <button className='btn btn-sm btn-primary' disabled={submittingComment}>
          {localization.comment.post}
        </button>
      </div>
    </form>
  );
}
