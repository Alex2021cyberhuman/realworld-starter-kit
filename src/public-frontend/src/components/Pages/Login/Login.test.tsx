import React from 'react';
import { Err, Ok } from '@hqoss/monads';
import { fireEvent, render, screen } from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { login } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { Login } from './Login';
import { startLoginIn, updateField } from './Login.slice';
import { MemoryRouter } from 'react-router-dom';

jest.mock('../../../services/webapi/conduit.ts');
jest.mock('axios');

const mockedLogin = login as jest.Mock<ReturnType<typeof login>>;

beforeEach(async () => {
  await act(async () => {
    await render(
      <MemoryRouter>
        <Login />
      </MemoryRouter>
    );
  });
});

it('Should change email', async () => {
  await act(async () => {
    await fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'test' } });
  });
  expect(store.getState().login.user.email).toMatch('test');
});

it('Should change password', async () => {
  await act(async () => {
    await fireEvent.change(screen.getByPlaceholderText('Password'), { target: { value: 'test pass' } });
  });
  expect(store.getState().login.user.password).toMatch('test pass');
});

it('Should have email and password values from store', async () => {
  await act(async () => {
    store.dispatch(updateField({ name: 'email', value: '1234' }));
    store.dispatch(updateField({ name: 'password', value: '5678' }));
  });

  expect(screen.getByPlaceholderText('Email')).toHaveValue('1234');
  expect(screen.getByPlaceholderText('Password')).toHaveValue('5678');
});

it('Should initialize on first render', async () => {
  await act(async () => {
    store.dispatch(updateField({ name: 'email', value: '1234' }));
    store.dispatch(updateField({ name: 'password', value: '34145' }));
  });

  expect(store.getState().login.user.email.length).toBe(4);
  expect(store.getState().login.user.password.length).toBe(5);
});

it('Should show errors if login fails and stop disabling the fields', async () => {
  mockedLogin.mockResolvedValueOnce(
    Err(
      new Map<string, string[]>([
        ['email', ['is invalid']],
        ['password', ['is empty']],
      ])
    )
  );

  await act(async () => {
    fireEvent.click(screen.getByRole('button'));
    expect(store.getState().login.loginIn).toBe(true);
  });

  expect(screen.getByText('is invalid')).toBeInTheDocument();
  expect(screen.getByText('is empty')).toBeInTheDocument();
  expect(screen.getByPlaceholderText('Email')).not.toBeDisabled();
  expect(screen.getByPlaceholderText('Password')).not.toBeDisabled();
});

it('Should disable fields during login', async () => {
  await act(async () => {
    await store.dispatch(startLoginIn());
  });

  expect(screen.getByPlaceholderText('Email')).toBeDisabled();
  expect(screen.getByPlaceholderText('Password')).toBeDisabled();
});

it('Should not try to login if it is already loging in', async () => {
  mockedLogin.mockResolvedValueOnce(
    Ok({
      email: 'jake@jake.jake',
      token: 'jwt.token.here',
      username: 'jake',
      bio: 'I work at statefarm',
      image: null,
    })
  );
  localStorage.clear();

  await act(async () => {
    store.dispatch(startLoginIn());
    fireEvent.click(screen.getByRole('button'));
  });

  expect(mockedLogin.mock.calls.length).toBe(0);
  mockedLogin.mockClear();
});
