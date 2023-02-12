import React from 'react';
import { Err, Ok } from '@hqoss/monads';
import { act, fireEvent, render, screen } from '@testing-library/react';
import { signUp } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { Register } from './Register';
import { initializeRegister } from './Register.slice';
import { MemoryRouter } from 'react-router-dom';

jest.mock('../../../services/webapi/conduit.ts');

beforeEach(() => {
  act(() => {
    store.dispatch(initializeRegister());
    render(
      <MemoryRouter>
        <Register />
      </MemoryRouter>
    );
  });
});

const mockedSignUp = signUp as jest.Mock<ReturnType<typeof signUp>>;

it('Should update user fields', async () => {
  await act(async () => {
    fireEvent.change(screen.getByPlaceholderText('Your Name'), { target: { value: 'testUsername' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'testEmail' } });
    fireEvent.change(screen.getByPlaceholderText('Password'), { target: { value: 'testPassword' } });
  });

  expect(store.getState().register.user.username).toMatch('testUsername');
  expect(store.getState().register.user.email).toMatch('testEmail');
  expect(store.getState().register.user.password).toMatch('testPassword');
});

it('Should show errors on failed sign up', async () => {
  mockedSignUp.mockResolvedValueOnce(
    Err(
      new Map<string, string[]>([
        ['email', ['is invalid']],
        ['password', ['is empty']],
      ])
    )
  );

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
  });

  expect(screen.getByText('is invalid')).toBeInTheDocument();
  expect(screen.getByText('is empty')).toBeInTheDocument();
});

it('Should change user on sign up', async () => {
  mockedSignUp.mockResolvedValueOnce(
    Ok({
      email: 'jake@jake.jakesettings',
      token: 'jwt.token.here',
      username: 'jake',
      bio: 'I work at statefarm',
      image: null,
    })
  );

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
  });
  expect(store.getState().app.user.unwrap().email).toMatch('jake@jake.jakesettings');
});
