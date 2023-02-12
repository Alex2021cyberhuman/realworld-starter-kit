import { store } from '../state/store';
import { Localization } from './localizations/enStrings';

const globalFeedTab = 'globalFeed';
const yourFeedTab = 'yourFeed';
const favoritedArticlesTab = 'favoritedArticles';
const myArticlesTab = 'myArticles';
const tabs = {
  globalFeedTab,
  yourFeedTab,
  favoritedArticlesTab,
  myArticlesTab,
};

export function getHomePageTabs(localization?: Localization | undefined) {
  const finalLocalization = localization || store.getState().app.localization;
  return new Map<string, string>([
    [globalFeedTab, finalLocalization.home.feed.globalFeed],
    [yourFeedTab, finalLocalization.home.feed.yourFeed],
  ]);
}

export function getProfilePageTabs(localization?: Localization | undefined) {
  const finalLocalization = localization || store.getState().app.localization;
  return new Map<string, string>([
    [favoritedArticlesTab, finalLocalization.profile.favoritedArticles],
    [myArticlesTab, finalLocalization.profile.myArticles],
  ]);
}

export default tabs;
