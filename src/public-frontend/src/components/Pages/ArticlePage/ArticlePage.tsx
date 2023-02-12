import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useStore } from '../../../state/storeHooks';
import { TagList } from '../../ArticlePreview/ArticlePreview';
import { useLocalization } from '../../../services/localizations/localization';
import { CommentSection } from './CommentSection';
import { ArticleMeta } from './ArticleMeta';
import { ArticlePageBanner } from './ArticlePageBanner';
import { onLoad } from './onLoad';
import { ArticleBody } from './ArticleBody';

export function ArticlePage() {
  const navigate = useNavigate();
  const { slug } = useParams<{ slug: string }>();
  const {
    articlePage: { article, commentSection, metaSection },
    app: { user },
  } = useStore(({ articlePage, app }) => ({
    articlePage,
    app,
  }));

  useEffect(() => {
    if (slug) {
      onLoad(slug, navigate);
    }
  }, [slug]);

  const { localization } = useLocalization();
  return article.match({
    none: () => <div>{localization.article.load}</div>,
    some: (article) => (
      <div className='article-page'>
        <ArticlePageBanner {...{ article, metaSection, user }} />

        <div className='container page'>
          <div className='row article-content'>
            <ArticleBody body={article.body} />
            <TagList tagList={article.tagList} />
          </div>

          <hr />

          <div className='article-actions'>
            <ArticleMeta {...{ article, metaSection, user }} />
          </div>

          <CommentSection {...{ user, commentSection, article }} />
        </div>
      </div>
    ),
  });
}
