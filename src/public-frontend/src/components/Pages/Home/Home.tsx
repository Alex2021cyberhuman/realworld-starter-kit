import React from 'react';
import { Option } from '@hqoss/monads';
import { getArticles, getFeed, getTags } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { useStoreWithInitializer } from '../../../state/storeHooks';
import { FeedFilters } from '../../../types/article';
import { ArticlesViewer } from '../../ArticlesViewer/ArticlesViewer';
import { changePage, loadArticles, startLoadingArticles } from '../../ArticlesViewer/ArticlesViewer.slice';
import { ContainerPage } from '../../ContainerPage/ContainerPage';
import { changeTab, loadTags, startLoadingTags } from './Home.slice';
import { useLocalization } from '../../../services/localizations/localization';
import tabs, { getHomePageTabs } from '../../../services/tabs';

export function Home() {
  const { tags, selectedTab } = useStoreWithInitializer(({ home }) => home, load);

  return (
    <div className='home-page'>
      {renderBanner()}
      <ContainerPage>
        <div className='col-md-9'>
          <ArticlesViewer
            toggleClassName='feed-toggle'
            tabsTranslation={getHomePageTabs()}
            selectedTab={selectedTab}
            tabs={buildTabsNames(selectedTab)}
            onPageChange={onPageChange}
            onTabChange={onTabChange}
          />
        </div>

        <div className='col-md-3'>
          <HomeSidebar tags={tags} />
        </div>
      </ContainerPage>
    </div>
  );
}

export async function load() {
  store.dispatch(startLoadingArticles());
  store.dispatch(startLoadingTags());

  if (store.getState().app.user.isSome()) {
    store.dispatch(changeTab(tabs.yourFeedTab));
  }

  const multipleArticles = await getFeedOrGlobalArticles();
  store.dispatch(loadArticles(multipleArticles));

  const tagsResult = await getTags();
  store.dispatch(loadTags(tagsResult.tags));
}

function renderBanner() {
  const { localization } = useLocalization();
  return (
    <div className='banner'>
      <div className='container'>
        <h1 className='logo-font'>{localization.home.banner.logoTitle}</h1>
        <p>{localization.home.banner.logoText}</p>
      </div>
    </div>
  );
}

function buildTabsNames(selectedTab: string) {
  const { user } = store.getState().app;

  return Array.from(new Set([...(user.isSome() ? [tabs.yourFeedTab] : []), tabs.globalFeedTab, selectedTab]));
}

async function onPageChange(index: number) {
  store.dispatch(changePage(index));

  const multipleArticles = await getFeedOrGlobalArticles({ offset: (index - 1) * 10 });
  store.dispatch(loadArticles(multipleArticles));
}

async function onTabChange(tab: string) {
  store.dispatch(changeTab(tab));
  store.dispatch(startLoadingArticles());

  const multipleArticles = await getFeedOrGlobalArticles();
  store.dispatch(loadArticles(multipleArticles));
}

async function getFeedOrGlobalArticles(filters: FeedFilters = {}) {
  const { selectedTab } = store.getState().home;
  const finalFilters = {
    ...filters,
    tag: selectedTab.slice(2),
  };

  const multipleArticles = await (selectedTab === tabs.yourFeedTab ? getFeed : getArticles)(
    !selectedTab.startsWith('#') ? filters : finalFilters
  );
  if (!multipleArticles) {
    throw new Error('Cannot load articles');
  }
  return multipleArticles;
}

function HomeSidebar({ tags }: { tags: Option<string[]> }) {
  const { localization } = useLocalization();
  return (
    <div className='sidebar'>
      <p>{localization.home.tags.popularTags}</p>

      {tags.match({
        none: () => <span>{localization.home.tags.load}</span>,
        some: (tags) => (
          <div className='tag-list'>
            {' '}
            {tags.map((tag) => (
              <a key={tag} href='#' className='tag-pill tag-default' onClick={() => onTabChange(`# ${tag}`)}>
                {tag}
              </a>
            ))}{' '}
          </div>
        ),
      })}
    </div>
  );
}
