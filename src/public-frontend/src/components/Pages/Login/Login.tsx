import React from 'react';
import { login } from '../../../services/webapi/conduit';
import { dispatchOnCall, store } from '../../../state/store';
import { useStoreWithInitializer } from '../../../state/storeHooks';
import { loadUserIntoApp } from '../../../types/user';
import { buildGenericFormField } from '../../../types/genericFormField';
import { GenericForm } from '../../GenericForm/GenericForm';
import { initializeLogin, LoginState, startLoginIn, updateErrors, updateField } from './Login.slice';
import { ContainerPage } from '../../ContainerPage/ContainerPage';
import { useLocalization } from '../../../services/localizations/localization';
import { NavigateFunction, useNavigate } from 'react-router-dom';

export function Login() {
  const { errors, loginIn, user } = useStoreWithInitializer(({ login }) => login, dispatchOnCall(initializeLogin()));

  const { localization } = useLocalization();
  const navigate = useNavigate();
  return (
    <div className='auth-page'>
      <ContainerPage>
        <div className='col-md-6 offset-md-3 col-xs-12'>
          <h1 className='text-xs-center'>{localization.login.pageHeader}</h1>
          <p className='text-xs-center'>
            <a href='/register'>{localization.login.registerOption}</a>
          </p>

          <GenericForm
            disabled={loginIn}
            formObject={user}
            submitButtonText='Sign in'
            errors={errors}
            onChange={onUpdateField}
            onSubmit={(ev) => signIn(ev, navigate)}
            fields={[
              buildGenericFormField({ name: 'email', placeholder: localization.login.email }),
              buildGenericFormField({ name: 'password', placeholder: localization.login.password, type: 'password' }),
            ]}
          />
        </div>
      </ContainerPage>
    </div>
  );
}

function onUpdateField(name: string, value: string) {
  store.dispatch(updateField({ name: name as keyof LoginState['user'], value }));
}

async function signIn(ev: React.FormEvent, navigate: NavigateFunction) {
  ev.preventDefault();

  if (store.getState().login.loginIn) return;
  store.dispatch(startLoginIn());

  const { email, password } = store.getState().login.user;
  const result = await login(email, password);

  result.match({
    ok: (user) => {
      loadUserIntoApp(user);
      navigate('/');
    },
    err: (e) => {
      store.dispatch(updateErrors(e));
    },
  });
}
