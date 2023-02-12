import React from 'react';
import { formatString, useLocalization } from '../../services/localizations/localization';

export function Footer() {
  const { localization } = useLocalization();
  return (
    <footer>
      <div className='container'>
        <a href='/#/' className='logo-font'>
          {localization.footer.logo}
        </a>
        <span className='attribution'>
          {formatString(
            localization.footer.attribution,
            <a href='https://github.com/alex2021cyberhuman-team'>alex2021cyberhuman-team</a>
          )}
        </span>
      </div>
    </footer>
  );
}
