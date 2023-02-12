import { Option } from '@hqoss/monads';
import { Article } from '../../../types/article';
import { User } from '../../../types/user';
import { MetaSectionState } from './ArticlePage.slice';
import { ArticleMeta } from './ArticleMeta';

export function ArticlePageBanner(props: { article: Article; metaSection: MetaSectionState; user: Option<User> }) {
  return (
    <div className='banner'>
      <div className='container'>
        <h1>{props.article.title}</h1>

        <ArticleMeta {...props} />
      </div>
    </div>
  );
}
