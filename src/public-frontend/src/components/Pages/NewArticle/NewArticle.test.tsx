import React from 'react';
import { Err } from '@hqoss/monads';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { createArticle } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { initializeEditor } from '../../ArticleEditor/ArticleEditor.slice';
import { NewArticle } from './NewArticle';
import { MemoryRouter } from 'react-router-dom';

jest.mock('../../../services/webapi/conduit.ts');

const mockedCreateArticle = createArticle as jest.Mock<ReturnType<typeof createArticle>>;

beforeEach(() => {
  act(() => {
    store.dispatch(initializeEditor());
    render(
      <MemoryRouter>
        <NewArticle />
      </MemoryRouter>
    );
  });
});

it('Should update errors if publish article fails', async () => {
  mockedCreateArticle.mockResolvedValueOnce(Err(new Map<string, string[]>([['title', ['too smol', 'much fun']]])));
  await act(async () => {
    fireEvent.click(screen.getByText('Publish Article'));
  });

  expect(screen.getByText('too smol')).toBeInTheDocument();
  expect(screen.getByText('much fun')).toBeInTheDocument();
});
