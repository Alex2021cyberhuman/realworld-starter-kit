import React from 'react';
import { act, cleanup, render, screen } from '@testing-library/react';
import { getArticles, getFeed, getTags, getUser } from '../../services/webapi/conduit';
import { store } from '../../state/store';
import { App } from './App';
import { reset } from './App.slice';

jest.mock('../../services/webapi/conduit.ts');
jest.mock('axios');

const mockedGetArticles = getArticles as jest.Mock<ReturnType<typeof getArticles>>;
const mockedGetFeed = getFeed as jest.Mock<ReturnType<typeof getFeed>>;
const mockedGetTags = getTags as jest.Mock<ReturnType<typeof getTags>>;
const mockedGetUser = getUser as jest.Mock<ReturnType<typeof getUser>>;
const testToken =
  'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjk5OTk5OTk5OTl9.Vg30C57s3l90JNap_VgMhKZjfc-p7SoBXaSAy8c28HA';
const defaultArticle = {
  author: {
    bio: null,
    following: false,
    image: 'https://static.productionready.io/images/smiley-cyrus.jpg',
    username: 'Jazmin Martinez',
  },
  body: 'Test 1',
  createdAt: new Date(),
  description: 'Test 1',
  favorited: false,
  favoritesCount: 0,
  slug: 'test-pmy91z',
  tagList: ['tag1', 'tag2'],
  title: 'Test',
  updatedAt: new Date(),
};

beforeEach(() => {
  store.dispatch(reset());
});

it('Should render home', async () => {
  mockedGetArticles.mockResolvedValueOnce({
    articles: [{ ...defaultArticle }],
    articlesCount: 1,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  mockedGetUser.mockImplementationOnce(jest.fn());
  localStorage.clear();

  await act(async () => {
    await render(<App />);
  });

  expect(screen.getByText('A place to share your knowledge.')).toBeInTheDocument();
  expect(mockedGetUser.mock.calls.length).toBe(0);
  mockedGetUser.mockClear();
  await cleanup();
});

it('Should end load if get user fails', async () => {
  mockedGetUser.mockRejectedValue({ lol: 123 });
  mockedGetArticles.mockResolvedValueOnce({
    articles: [{ ...defaultArticle }],
    articlesCount: 1,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });

  await act(async () => {
    await render(<App />);
  });

  expect(store.getState().app.user.isNone()).toBeTruthy();
  expect(store.getState().app.loading).toBe(false);
  await cleanup();
});

it('Should get user if token is on storage', async () => {
  mockedGetUser.mockResolvedValueOnce({
    email: 'jake@jake.jake',
    token: testToken,
    username: 'jake',
    bio: 'I work at statefarm',
    image: null,
  });
  mockedGetFeed.mockResolvedValueOnce({
    articles: [{ ...defaultArticle }],
    articlesCount: 1,
  });
  mockedGetTags.mockResolvedValueOnce({ tags: [] });
  localStorage.setItem('token', testToken);

  await act(async () => {
    await render(<App />);
  });

  expect(store.getState().app.user.isSome()).toBeTruthy();
  expect(store.getState().app.loading).toBe(false);
  await cleanup();
});
