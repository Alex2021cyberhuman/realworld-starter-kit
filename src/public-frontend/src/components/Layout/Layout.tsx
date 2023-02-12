import { useStore } from '../../state/storeHooks';
import { Outlet } from 'react-router-dom';
import React, { Fragment } from 'react';
import { Header } from '../Header/Header';
import { Footer } from '../Footer/Footer';

export function Layout() {
  const { loading } = useStore(({ app }) => app);
  return (
    <Fragment>
      {!loading && (
        <Fragment>
          <Header />
          <Outlet />
          <Footer />
        </Fragment>
      )}
    </Fragment>
  );
}
