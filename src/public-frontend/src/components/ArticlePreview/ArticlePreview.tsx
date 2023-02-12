import React from 'react';
import { Link } from 'react-router-dom';
import { Article } from '../../types/article';
import { getLocalDate, useLocalization } from '../../services/localizations/localization';
import { SmallAvatar } from '../Avatars/SmallAvatar';

export function ArticlePreview({
  article: {
    createdAt,
    favorited,
    favoritesCount,
    slug,
    title,
    description,
    tagList,
    author: { image, username },
  },
  isSubmitting,
  onFavoriteToggle,
}: {
  article: Article;
  isSubmitting: boolean;
  onFavoriteToggle?: () => void;
}) {
  const { localization, language } = useLocalization();
  return (
    <div className='article-preview'>
      <div className='article-meta'>
        <Link to={`/profile/${username}`} className='author'>
          <SmallAvatar image={image} />
        </Link>
        <div className='info'>
          <Link to={`/profile/${username}`} className='author'>
            {username}
          </Link>
          <span className='date'>{getLocalDate(createdAt, language)}</span>
        </div>
        <button
          className={`btn btn-sm pull-xs-right ${favorited ? 'btn-primary' : 'btn-outline-primary'}`}
          aria-label={localization.article.toggleFavorited}
          disabled={isSubmitting}
          onClick={onFavoriteToggle}
        >
          <i className='ion-heart'></i> {favoritesCount}
        </button>
      </div>
      <a href={`/article/${slug}`} className='preview-link'>
        <h1>{title}</h1>
        <p>{description}</p>
        <span>{localization.article.readMore}</span>
        <TagList tagList={tagList} />
      </a>
    </div>
  );
}

export function TagList({ tagList }: { tagList: string[] }) {
  return (
    <ul className='tag-list'>
      {tagList.map((tag) => (
        <li key={tag} className='tag-default tag-pill tag-outline'>
          {tag}
        </li>
      ))}
    </ul>
  );
}
