import { AxiosRequestConfig } from 'axios';
import { store } from '../../state/store';

export function setLanguageRequest(request: AxiosRequestConfig<unknown>) {
  const state = store.getState();
  const languageCode = state.app.language;
  if (request.headers) {
    request.headers['Accept-Language'] = languageCode;
  } else {
    request.headers = { 'Accept-Language': languageCode };
  }
  return request;
}
