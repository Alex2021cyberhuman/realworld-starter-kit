import React, { Fragment } from 'react';
import { favoriteArticle, unfavoriteArticle } from '../../services/webapi/conduit';
import { store } from '../../state/store';
import { useStore } from '../../state/storeHooks';
import { Article } from '../../types/article';
import { classObjectToClassName } from '../../types/style';
import { ArticlePreview } from '../ArticlePreview/ArticlePreview';
import { Pagination } from '../Pagination/Pagination';
import { ArticleViewerState, endSubmittingFavorite, startSubmittingFavorite } from './ArticlesViewer.slice';
import { useLocalization } from '../../services/localizations/localization';
import { NavigateFunction, useNavigate } from 'react-router-dom';

export function ArticlesViewer({
  toggleClassName,
  tabs,
  tabsTranslation,
  selectedTab,
  onPageChange,
  onTabChange,
}: {
  toggleClassName: string;
  tabs: string[];
  tabsTranslation: Map<string, string>;
  selectedTab: string;
  onPageChange?: (index: number) => void;
  onTabChange?: (tab: string) => void;
}) {
  const { articles, articlesCount, currentPage } = useStore(({ articleViewer }) => articleViewer);

  return (
    <Fragment>
      <ArticlesTabSet {...{ tabs, selectedTab, tabsTranslation, toggleClassName, onTabChange }} />
      <ArticleList articles={articles} />
      <Pagination currentPage={currentPage} count={articlesCount} itemsPerPage={10} onPageChange={onPageChange} />
    </Fragment>
  );
}

function ArticlesTabSet({
  tabs,
  toggleClassName,
  selectedTab,
  tabsTranslation,
  onTabChange,
}: {
  tabs: string[];
  toggleClassName: string;
  selectedTab: string;
  tabsTranslation: Map<string, string>;
  onTabChange?: (tab: string) => void;
}) {
  return (
    <div className={toggleClassName}>
      <ul className='nav nav-pills outline-active'>
        {tabs.map((tab) => (
          <Tab
            key={tab}
            tab={tab}
            tabName={tabsTranslation.get(tab)}
            active={tab === selectedTab}
            onClick={() => onTabChange && onTabChange(tab)}
          />
        ))}
      </ul>
    </div>
  );
}

function Tab({
  tab,
  tabName,
  active,
  onClick,
}: {
  tab: string;
  tabName: string | undefined;
  active: boolean;
  onClick: () => void;
}) {
  return (
    <li className='nav-item'>
      <a
        className={classObjectToClassName({ 'nav-link': true, active })}
        href='#'
        onClick={(ev) => {
          ev.preventDefault();
          onClick();
        }}
      >
        {tabName || tab}
      </a>
    </li>
  );
}

function ArticleList({ articles }: { articles: ArticleViewerState['articles'] }) {
  const { localization } = useLocalization();
  const navigate = useNavigate();
  return articles.match({
    none: () => (
      <div className='article-preview' key={1}>
        {localization.viewer.loading}
      </div>
    ),
    some: (articles) => (
      <Fragment>
        {articles.length === 0 && (
          <div className='article-preview' key={1}>
            {localization.viewer.notArticles}
          </div>
        )}
        {articles.map(({ article, isSubmitting }, index) => (
          <ArticlePreview
            key={article.slug}
            article={article}
            isSubmitting={isSubmitting}
            onFavoriteToggle={isSubmitting ? undefined : onFavoriteToggle(index, article, navigate)}
          />
        ))}
      </Fragment>
    ),
  });
}

function onFavoriteToggle(index: number, { slug, favorited }: Article, navigate: NavigateFunction) {
  return async () => {
    if (store.getState().app.user.isNone()) {
      navigate('/login');
      return;
    }
    store.dispatch(startSubmittingFavorite(index));

    await (favorited ? unfavoriteArticle(slug) : favoriteArticle(slug));
    store.dispatch(endSubmittingFavorite({ index, favorited: favorited }));
  };
}
