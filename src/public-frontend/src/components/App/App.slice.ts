import { None, Option, Some } from '@hqoss/monads';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../../types/user';
import { scheduleRefreshToken } from '../../services/webapi/scheduleRefreshToken';
import { evolve, F } from 'ramda';
import { enStrings, Localization } from '../../services/localizations/enStrings';
import { languagesDictionaries } from '../../services/localizations/localization';

export interface AppState {
  user: Option<User>;
  loading: boolean;
  language: string;
  localization: Localization;
}

const initialState: AppState = {
  user: None,
  loading: true,
  language: 'en',
  localization: enStrings,
};

const slice = createSlice({
  name: 'app',
  initialState,
  reducers: {
    reset: (state) => {
      state.user = initialState.user;
      state.loading = initialState.loading;
      state.language = initialState.language;
      state.localization = initialState.localization;
    },
    loadLanguage: (state) => {
      const language = localStorage.getItem('languageCode') || state.language;
      const languagesDictionary = languagesDictionaries.get(language);
      if (languagesDictionary) {
        state.language = language;
        state.localization = languagesDictionary;
        localStorage.setItem('languageCode', language);
      }
    },
    loadUser: (state, { payload: user }: PayloadAction<User>) => {
      scheduleRefreshToken(user);
      state.user = Some(user);
      state.loading = false;
    },
    changeLanguage: (state, { payload: languageCode }: PayloadAction<string>) => {
      const languageDictionary = languagesDictionaries.get(languageCode);
      if (languageDictionary) {
        localStorage.setItem('languageCode', languageCode);
        state.language = languageCode;
        state.localization = languageDictionary;
      }
    },
    logout: (state) => {
      state.user = None;
      localStorage.removeItem('token');
    },
    endLoad: (state) => evolve({ loading: F })(state),
  },
});

export const { loadUser, logout, endLoad, loadLanguage, changeLanguage, reset } = slice.actions;

export default slice.reducer;
