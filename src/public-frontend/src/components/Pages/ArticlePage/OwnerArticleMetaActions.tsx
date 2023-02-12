import { Fragment } from 'react';
import { useNavigate } from 'react-router-dom';
import { Article } from '../../../types/article';
import { useLocalization } from '../../../services/localizations/localization';
import { onDeleteArticle } from './onDeleteArticle';

export function OwnerArticleMetaActions({
  article: { slug },
  deletingArticle,
}: {
  article: Article;
  deletingArticle: boolean;
}) {
  const { localization } = useLocalization();
  const navigate = useNavigate();
  return (
    <Fragment>
      <button className='btn btn-outline-secondary btn-sm' onClick={() => navigate(`/editor/${slug}`)}>
        <i className='ion-plus-round'></i>
        &nbsp; {localization.article.edit}
      </button>
      &nbsp;
      <button
        className='btn btn-outline-danger btn-sm'
        disabled={deletingArticle}
        onClick={() => onDeleteArticle(slug, navigate)}
      >
        <i className='ion-heart'></i>
        &nbsp; {localization.article.delete}
      </button>
    </Fragment>
  );
}
