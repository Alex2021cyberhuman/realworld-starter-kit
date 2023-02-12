import React from 'react';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import {
  createComment,
  deleteArticle,
  deleteComment,
  favoriteArticle,
  followUser,
  getArticle,
  getArticleComments,
  unfavoriteArticle,
  unfollowUser,
} from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { Comment } from '../../../types/comment';
import { loadUser } from '../../App/App.slice';
import { ArticlePage } from './ArticlePage';

jest.mock('../../../services/webapi/conduit.ts');

const mockedGetArticle = getArticle as jest.Mock<ReturnType<typeof getArticle>>;
const mockedGetArticleComments = getArticleComments as jest.Mock<ReturnType<typeof getArticleComments>>;
const mockedFollowUser = followUser as jest.Mock<ReturnType<typeof followUser>>;
const mockedUnfollowUser = unfollowUser as jest.Mock<ReturnType<typeof unfollowUser>>;
const mockedFavoriteArticle = favoriteArticle as jest.Mock<ReturnType<typeof favoriteArticle>>;
const mockedUnfavoriteArticle = unfavoriteArticle as jest.Mock<ReturnType<typeof unfavoriteArticle>>;
const mockedCreateComment = createComment as jest.Mock<ReturnType<typeof createComment>>;
const mockedDeleteComment = deleteComment as jest.Mock<ReturnType<typeof deleteComment>>;
const mockedDeleteArticle = deleteArticle as jest.Mock<ReturnType<typeof deleteArticle>>;

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
  slug: 'sample-slug',
  tagList: [],
  title: 'Test',
  updatedAt: new Date(),
};

const defaultComment: Comment = {
  id: '1',
  createdAt: new Date(),
  updatedAt: new Date(),
  body: 'It takes a Jacobian',
  author: {
    username: 'jake',
    bio: 'I work at statefarm',
    image: 'https://i.stack.imgur.com/xHWG8.jpg',
    following: false,
  },
};

async function renderWithPath(slug: string) {
  await act(async () => {
    render(
      <MemoryRouter initialEntries={[`/article/${slug}`]}>
        <Routes>
          <Route path='/article/:slug' element={<ArticlePage />} />
          <Route path='/' element={<ArticlePage />} />
        </Routes>
      </MemoryRouter>
    );
  });
}

describe('For non article owner User', () => {
  beforeEach(async () => {
    await act(async () => {
      store.dispatch(
        loadUser({
          email: 'jake@jake.jake',
          token: 'jwt.token.here',
          username: 'jake2',
          bio: 'I work at statefarm',
          image: null,
        })
      );
    });
  });

  it('Should not show sign in option', async () => {
    mockedGetArticle.mockResolvedValueOnce(defaultArticle);
    mockedGetArticleComments.mockResolvedValueOnce([defaultComment]);
    await renderWithPath('sample-slug');

    expect(screen.queryAllByText('Sign in')).toHaveLength(0);
  });

  it('Should follow user and rerender', async () => {
    mockedGetArticle.mockResolvedValueOnce({
      ...defaultArticle,
      author: { ...defaultArticle.author, username: 'the truth', following: false },
    });
    mockedGetArticleComments.mockResolvedValueOnce([]);
    await renderWithPath('sample-slug');

    mockedFollowUser.mockResolvedValueOnce({ ...defaultArticle.author, following: true, username: 'the truth' });
    await act(async () => {
      fireEvent.click(screen.queryAllByText('Follow the truth')[0]);
    });

    expect(mockedFollowUser.mock.calls).toHaveLength(1);
    expect(screen.queryAllByText('Unfollow the truth')[0]).toBeInTheDocument();
  });

  it('Should unfollow user and rerender', async () => {
    mockedGetArticle.mockResolvedValueOnce({
      ...defaultArticle,
      author: { ...defaultArticle.author, username: 'the truth', following: true },
    });
    mockedGetArticleComments.mockResolvedValueOnce([]);
    await renderWithPath('sample-slug');

    mockedUnfollowUser.mockResolvedValueOnce({ ...defaultArticle.author, following: false, username: 'the truth' });
    await act(async () => {
      fireEvent.click(screen.queryAllByText('Unfollow the truth')[0]);
    });

    expect(mockedUnfollowUser.mock.calls).toHaveLength(1);
    expect(screen.queryAllByText('Follow the truth')[0]).toBeInTheDocument();
  });

  it('Should favorite article', async () => {
    mockedGetArticle.mockResolvedValueOnce({
      ...defaultArticle,
      favorited: false,
    });
    mockedGetArticleComments.mockResolvedValueOnce([]);
    await renderWithPath('sample-slug');

    mockedFavoriteArticle.mockResolvedValueOnce();
    await act(async () => {
      fireEvent.click(screen.queryAllByText('Favorite Article')[0]);
    });

    expect(mockedFavoriteArticle.mock.calls).toHaveLength(1);
    expect(screen.queryAllByText('Unfavorite Article')[0]).toBeInTheDocument();
  });

  it('Should unfavorite article', async () => {
    mockedGetArticle.mockResolvedValueOnce({
      ...defaultArticle,
      favorited: true,
    });
    mockedGetArticleComments.mockResolvedValueOnce([]);
    await renderWithPath('sample-slug');

    mockedUnfavoriteArticle.mockResolvedValueOnce();
    await act(async () => {
      fireEvent.click(screen.queryAllByText('Unfavorite Article')[0]);
    });

    expect(mockedUnfavoriteArticle.mock.calls).toHaveLength(1);
    expect(screen.queryAllByText('Favorite Article')[0]).toBeInTheDocument();
  });

  it('Should create comment', async () => {
    mockedGetArticle.mockResolvedValueOnce(defaultArticle);
    mockedGetArticleComments.mockResolvedValueOnce([]);
    await renderWithPath('sample-slug');

    mockedCreateComment.mockResolvedValueOnce(defaultComment);
    mockedGetArticleComments.mockResolvedValueOnce([{ ...defaultComment, body: 'This is a test comment' }]);
    await act(async () => {
      fireEvent.change(screen.getByPlaceholderText('Write a comment...'), {
        target: { value: 'This is a test comment' },
      });
    });

    await act(async () => {
      fireEvent.click(screen.getByText('Post Comment'));
    });

    expect(screen.getByText('This is a test comment')).toBeInTheDocument();
    expect(mockedCreateComment.mock.calls).toHaveLength(1);
    expect(mockedCreateComment.mock.calls[0][0]).toMatch(defaultArticle.slug);
    expect(mockedCreateComment.mock.calls[0][1]).toMatch('This is a test comment');
  });

  it("Comment should not have delete button if is not the logged user's comment", async () => {
    mockedGetArticle.mockResolvedValueOnce(defaultArticle);
    mockedGetArticleComments.mockResolvedValueOnce([
      {
        ...defaultComment,
        id: '3',
        body: 'This is a test comment',
        author: { ...defaultComment.author, username: 'jake0' },
      },
    ]);
    await renderWithPath('sample-slug');

    expect(screen.queryAllByLabelText('Delete comment 1')).toHaveLength(0);
  });

  it('Should delete comment', async () => {
    mockedGetArticle.mockResolvedValueOnce(defaultArticle);
    mockedGetArticleComments.mockResolvedValueOnce([
      {
        ...defaultComment,
        id: '3',
        body: 'This is a test comment',
        author: { ...defaultComment.author, username: 'jake2', image: '' },
      },
    ]);
    await renderWithPath('sample-slug');

    mockedDeleteComment.mockResolvedValueOnce();
    mockedGetArticleComments.mockResolvedValueOnce([{ ...defaultComment, body: 'This is a test comment after' }]);
    await act(async () => {
      fireEvent.click(screen.getByLabelText('Delete comment 1'));
    });

    expect(screen.getByText('This is a test comment after')).toBeInTheDocument();
    expect(mockedDeleteComment.mock.calls).toHaveLength(1);
    expect(mockedDeleteComment.mock.calls[0][0]).toMatch(defaultArticle.slug);
    expect(mockedDeleteComment.mock.calls[0][1]).toBe('3');
  });
});

describe('For article owner User', () => {
  beforeEach(async () => {
    await act(async () => {
      store.dispatch(
        loadUser({
          email: 'jake@jake.jake',
          token: 'jwt.token.here',
          username: 'jake3',
          bio: 'I work at statefarm',
          image: null,
        })
      );
    });
  });

  it('Should delete article and redirect to homepage', async () => {
    mockedGetArticle.mockResolvedValueOnce({
      ...defaultArticle,
      author: { ...defaultArticle.author, username: 'jake3' },
    });
    mockedGetArticleComments.mockResolvedValueOnce([defaultComment]);
    await renderWithPath(defaultArticle.slug);

    mockedDeleteArticle.mockResolvedValueOnce();
    await act(async () => {
      fireEvent.click(screen.queryAllByText('Delete Article')[0]);
    });

    expect(location.pathname).toMatch(`/`);
    expect(mockedDeleteArticle.mock.calls).toHaveLength(1);
  });
});
