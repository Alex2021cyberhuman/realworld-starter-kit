import React, { Fragment } from 'react';
import { NavLink } from 'react-router-dom';
import { useStore } from '../../state/storeHooks';
import { User } from '../../types/user';
import { languagesTranslates, useLocalization } from '../../services/localizations/localization';
import { store } from '../../state/store';
import { changeLanguage } from '../App/App.slice';

export function Header() {
  const { user } = useStore(({ app }) => app);

  const { localization } = useLocalization();
  return (
    <nav className='navbar navbar-light'>
      <div className='container'>
        <a className='navbar-brand' href='/'>
          {localization.header.logoTitle}
        </a>

        <ul className='nav navbar-nav pull-xs-left'>
          <Languages />
        </ul>
        <ul className='nav navbar-nav pull-xs-right'>
          <NavItem text={localization.header.homePage} href='/' />

          {user.match({
            none: () => <GuestLinks />,
            some: (user) => <UserLinks user={user} />,
          })}
        </ul>
      </div>
    </nav>
  );
}

function NavItem({ text, href, icon }: { text: string; href: string; icon?: string }) {
  return (
    <li className='nav-item'>
      <NavLink to={href} className={({ isActive }) => (isActive ? 'active nav-link' : 'nav-link')}>
        {icon && <i className={icon}></i>}&nbsp;{text}
      </NavLink>
    </li>
  );
}

function GuestLinks() {
  const { localization } = useLocalization();
  return (
    <Fragment>
      <NavItem text={localization.header.login} href='/login' />
      <NavItem text={localization.header.register} href='/register' />
    </Fragment>
  );
}

function UserLinks({ user: { username } }: { user: User }) {
  const { localization } = useLocalization();
  return (
    <Fragment>
      <NavItem text={localization.header.newArticle} href='/editor' icon='ion-compose' />
      <NavItem text={localization.header.settings} href='/settings' icon='ion-gear-a' />
      <NavItem text={`${username}`} href={`/profile/${username}`} />
    </Fragment>
  );
}

function Languages() {
  const { language } = useLocalization();
  function setLanguage(code: string) {
    const language = store.getState().app.language;
    if (code !== language) {
      store.dispatch(changeLanguage(code));
    }
  }

  function onSetLanguageClick(code: string) {
    setLanguage(code);
  }

  const currentLanguageCode = language;
  const languages = [...languagesTranslates.keys()];
  return (
    <>
      {languages.map((code) => {
        const className = 'nav-link' + (currentLanguageCode === code ? ' active' : '');
        return (
          <li className='nav-item' key={`language-li-${code}`} id={code}>
            <a
              id={`set-language-${code}`}
              href={`#${code}`}
              className={className}
              onClick={() => onSetLanguageClick(code)}
            >
              {languagesTranslates.get(code)}
            </a>
          </li>
        );
      })}
    </>
  );
}
