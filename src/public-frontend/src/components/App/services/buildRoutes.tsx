import React from 'react';
import { Navigate, RouteObject } from 'react-router-dom';
import { Layout } from '../../Layout/Layout';
import { State } from '../../../state/store';
import { ProfilePage } from '../../Pages/ProfilePage/ProfilePage';
import { Settings } from '../../Pages/Settings/Settings';
import { ArticlePage } from '../../Pages/ArticlePage/ArticlePage';
import { EditArticle } from '../../Pages/EditArticle/EditArticle';
import { Home } from '../../Pages/Home/Home';
import { Login } from '../../Pages/Login/Login';
import { NewArticle } from '../../Pages/NewArticle/NewArticle';
import { Register } from '../../Pages/Register/Register';

export function buildRoutes(state: State) {
  let layoutRoutes: RouteObject[] = [
    { index: true, element: <Home /> },
    {
      path: '/profile/:username',
      element: <ProfilePage />,
    },
    {
      path: '/profile/:username/:favorites',
      element: <ProfilePage />,
    },
    {
      path: '/article/:slug',
      element: <ArticlePage />,
    },
  ];
  const userIsLogged = state.app.user.isSome();
  if (userIsLogged) {
    layoutRoutes = layoutRoutes.concat([
      {
        path: '/settings',
        element: <Settings />,
      },
      {
        index: true,
        path: '/editor',
        element: <NewArticle />,
      },
      {
        path: '/editor/:slug',
        element: <EditArticle />,
      },
    ]);
  } else {
    layoutRoutes = layoutRoutes.concat([
      {
        path: '/login',
        element: <Login />,
      },
      {
        path: '/register',
        element: <Register />,
      },
    ]);
  }
  layoutRoutes = layoutRoutes.concat([{ path: '*', element: <Navigate to='/' /> }]);

  const result: RouteObject[] = [
    {
      path: '/',
      element: <Layout />,
      children: [...layoutRoutes],
    },
  ];

  return result;
}
