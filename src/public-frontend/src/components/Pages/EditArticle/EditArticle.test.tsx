import React from 'react';
import { Err } from '@hqoss/monads';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { getArticle, updateArticle } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { loadUser } from '../../App/App.slice';
import { initializeEditor } from '../../ArticleEditor/ArticleEditor.slice';
import { EditArticle } from './EditArticle';

jest.mock('../../../services/webapi/conduit.ts');

const mockedGetArticle = getArticle as jest.Mock<ReturnType<typeof getArticle>>;
const mockedUpdateArticle = updateArticle as jest.Mock<ReturnType<typeof updateArticle>>;

const defaultArticle = {
  author: {
    bio: null,
    following: false,
    image: null,
    username: 'jake',
  },
  body: 'Test 1',
  createdAt: new Date(),
  description: 'Test 1',
  favorited: false,
  favoritesCount: 0,
  slug: 'test-pmy91z',
  tagList: [],
  title: 'Test',
  updatedAt: new Date(),
};

const defaultUser = {
  email: 'jake@jake.jake',
  token: 'jwt.token.here',
  username: 'jake',
  bio: 'I work at statefarm',
  image: null,
};

beforeEach(async () => {
  await act(async () => {
    store.dispatch(loadUser(defaultUser));
    store.dispatch(initializeEditor());
  });
});

async function renderWithPath(articleSlug: string) {
  await act(async () => {
    render(
      <MemoryRouter initialEntries={[`/editor/${articleSlug}`]}>
        <Routes>
          <Route path='/editor/:slug' element={<EditArticle />} />
        </Routes>
      </MemoryRouter>
    );
  });
}

it('Should load article', async () => {
  mockedGetArticle.mockResolvedValueOnce({ ...defaultArticle, tagList: ['123', '456'] });
  await renderWithPath('1234');

  expect(store.getState().editor.loading).toBeFalsy();
  expect(store.getState().editor.article.tagList).toHaveLength(2);
  expect(store.getState().editor.article.title).toMatch(defaultArticle.title);
});

it('Should update errors if publish article fails', async () => {
  mockedGetArticle.mockResolvedValueOnce(defaultArticle);
  mockedUpdateArticle.mockResolvedValueOnce(Err(new Map<string, string[]>([['title', ['too smol', 'much fun']]])));

  await renderWithPath('1234');
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(screen.getByText('too smol')).toBeInTheDocument();
  expect(screen.getByText('much fun')).toBeInTheDocument();
});
