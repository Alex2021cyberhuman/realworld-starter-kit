import axios from 'axios';
import { ArticleImage } from '../../components/FormGroup/MarkdownFormField';
import settings from '../../config/settings';

axios.defaults.baseURL = settings.baseApiUrl;

export function removeArticleImage(id: string): Promise<void> {
  return Promise.resolve();
}

export function uploadArticleImage(image: File): Promise<ArticleImage> {
  return Promise.resolve({
    id: '159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868',
    url: 'https://user-images.githubusercontent.com/59359082/159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868.png',
    mediaType: 'image/png',
  });
}

export function assingImagesToArticle(articleId: string, ids: string[]) {
  return Promise.resolve();
}

export function getArticleImages(articleId: string): Promise<ArticleImage[]> {
  return Promise.resolve([
    {
      id: '159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868',
      articleId: articleId,
      url: 'https://user-images.githubusercontent.com/59359082/159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868.png',
      mediaType: 'image/png',
    },
    {
      id: '159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868',
      articleId: articleId,
      url: 'https://user-images.githubusercontent.com/59359082/159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868.png',
      mediaType: 'image/png',
    },
    {
      id: '159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868',
      articleId: articleId,
      url: 'https://user-images.githubusercontent.com/59359082/159666542-fb3210bf-5ddc-4c35-8632-a93cc3a52868.png',
      mediaType: 'image/png',
    },
  ]);
}
