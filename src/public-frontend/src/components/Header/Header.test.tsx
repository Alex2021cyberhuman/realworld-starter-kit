import { act, render, screen } from '@testing-library/react';
import { store } from '../../state/store';
import { changeLanguage, loadLanguage, loadUser } from '../App/App.slice';
import { Header } from './Header';
import React from 'react';
import { MemoryRouter } from 'react-router-dom';

beforeEach(() => {
  render(
    <MemoryRouter>
      <Header />
    </MemoryRouter>
  );
});

it('Should render', () => {});

describe('Header for guest', () => {
  beforeAll(() => {
    act(() => {
      store.dispatch(changeLanguage('en'));
      store.dispatch(loadLanguage);
    });
  });

  it('Should render Sign in link', () => {
    expectLinkByText('Sign in', 'login');
  });

  it('Should render Sign up link', () => {
    expectLinkByText('Sign up', 'register');
  });

  it('Should not render New Article link', () => {
    expectEmptyQueryByText('New Article');
  });

  it('Should not render Settings link', () => {
    expectEmptyQueryByText('Settings');
  });
});

describe('Header for russian speaking guest', () => {
  beforeAll(() => {
    act(() => {
      store.dispatch(changeLanguage('ru'));
      store.dispatch(loadLanguage);
    });
  });

  it('Should render Sign in link', () => {
    expectLinkByText('Войти', 'login');
  });

  it('Should render Sign up link', () => {
    expectLinkByText('Зарегистрироваться', 'register');
  });

  it('Should not render New Article link', () => {
    expectEmptyQueryByText('Новая статья');
  });

  it('Should not render Settings link', () => {
    expectEmptyQueryByText('Настройки');
  });
});

describe('Header for russian speaking user', () => {
  beforeAll(() => {
    act(() => {
      store.dispatch(
        loadUser({
          email: 'jake@jake.jake',
          token: 'jwt.token.here',
          username: 'jake',
          bio: 'I work at statefarm',
          image: null,
        })
      );
    });
  });

  it('Should render New Article link', () => {
    expectLinkByText('Новая статья', 'editor');
  });

  it('Should render Settings link', () => {
    expectLinkByText('Настройки', 'settings');
  });

  it('Should render user link', () => {
    expectLinkByText('jake', 'profile/jake');
  });

  it('Should not render Sign in link', () => {
    expectEmptyQueryByText('Войти');
  });

  it('Should not render Sign up link', () => {
    expectEmptyQueryByText('Зарегистрироваться');
  });
});

describe('Header for user', () => {
  beforeAll(() => {
    act(() => {
      store.dispatch(changeLanguage('en'));
      store.dispatch(
        loadUser({
          email: 'jake@jake.jake',
          token: 'jwt.token.here',
          username: 'jake',
          bio: 'I work at statefarm',
          image: null,
        })
      );
    });
  });

  it('Should render New Article link', () => {
    expectLinkByText('New Article', 'editor');
  });

  it('Should render Settings link', () => {
    expectLinkByText('Settings', 'settings');
  });

  it('Should render user link', () => {
    expectLinkByText('jake', 'profile/jake');
  });

  it('Should not render Sign in link', () => {
    expectEmptyQueryByText('Sign in');
  });

  it('Should not render Sign up link', () => {
    expectEmptyQueryByText('Sign up');
  });
});

function expectLinkByText(text: string, href: string) {
  const link = screen.getByText(text);
  expect(link).toBeInTheDocument();
  expect(link.nodeName).toMatch('A');
  expect(link.getAttribute('href')).toMatch('/' + href);
}

function expectEmptyQueryByText(text: string) {
  expect(screen.queryAllByText(text)).toHaveLength(0);
}
