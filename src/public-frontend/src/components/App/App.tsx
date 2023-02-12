import { BrowserRouter, useRoutes } from 'react-router-dom';
import { useStoreWithInitializer } from '../../state/storeHooks';
import { load } from './services/load';
import { buildRoutes } from './services/buildRoutes';
import { Fragment } from 'react';
import { State } from '../../state/store';

export function App() {
  const state = useStoreWithInitializer((state) => state, load);
  return (
    <BrowserRouter>
      <Routing state={state} />
    </BrowserRouter>
  );
}

export function Routing({ state }: { state: State }) {
  const routes = buildRoutes(state);
  const element = useRoutes(routes);
  return <Fragment>{element}</Fragment>;
}
