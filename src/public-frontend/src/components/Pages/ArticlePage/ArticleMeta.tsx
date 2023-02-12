import { Option } from '@hqoss/monads';
import { Article } from '../../../types/article';
import { User } from '../../../types/user';
import { MetaSectionState } from './ArticlePage.slice';
import { OwnerArticleMetaActions } from './OwnerArticleMetaActions';
import { NonOwnerArticleMetaActions } from './NonOwnerArticleMetaActions';
import { ArticleAuthorInfo } from './ArticleAuthorInfo';

export function ArticleMeta({
  article,
  metaSection: { submittingFavorite, submittingFollow, deletingArticle },
  user,
}: {
  article: Article;
  metaSection: MetaSectionState;
  user: Option<User>;
}) {
  return (
    <div className='article-meta'>
      <ArticleAuthorInfo article={article} />

      {user.isSome() && user.unwrap().username === article.author.username ? (
        <OwnerArticleMetaActions article={article} deletingArticle={deletingArticle} />
      ) : (
        <NonOwnerArticleMetaActions
          article={article}
          submittingFavorite={submittingFavorite}
          submittingFollow={submittingFollow}
        />
      )}
    </div>
  );
}
