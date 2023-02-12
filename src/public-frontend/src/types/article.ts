import { array, boolean, Decoder, iso8601, number, object, string, uuid } from 'decoders';
import { Profile, profileDecoder } from './profile';

export interface Article {
  id: string;
  slug: string;
  title: string;
  description: string;
  body: string;
  tagList: string[];
  createdAt: Date;
  updatedAt: Date;
  favorited: boolean;
  favoritesCount: number;
  author: Profile;
}

export const articleDecoder: Decoder<Article> = object({
  id: uuid,
  slug: string,
  title: string,
  description: string,
  body: string,
  tagList: array(string),
  createdAt: iso8601,
  updatedAt: iso8601,
  favorited: boolean,
  favoritesCount: number,
  author: profileDecoder,
});

export interface MultipleArticles {
  articles: Article[];
  articlesCount: number;
}

export const multipleArticlesDecoder: Decoder<MultipleArticles> = object({
  articles: array(articleDecoder),
  articlesCount: number,
});

export interface ArticleForEditor {
  title: string;
  description: string;
  body: string;
  tagList: string[];
}

export interface ArticlesFilters {
  tag?: string;
  author?: string;
  favorited?: string;
  limit?: number;
  offset?: number;
}

export interface FeedFilters {
  limit?: number;
  offset?: number;
}
