import React from 'react';
import { dispatchOnCall, store } from '../../../state/store';
import { useStoreWithInitializer } from '../../../state/storeHooks';
import { buildGenericFormField } from '../../../types/genericFormField';
import { GenericForm } from '../../GenericForm/GenericForm';
import { initializeRegister, RegisterState, startSigningUp, updateErrors, updateField } from './Register.slice';
import { loadUserIntoApp, UserForRegistration } from '../../../types/user';
import { signUp } from '../../../services/webapi/conduit';
import { ContainerPage } from '../../ContainerPage/ContainerPage';
import { useLocalization } from '../../../services/localizations/localization';
import { NavigateFunction, useNavigate } from 'react-router-dom';

export function Register() {
  const { errors, signingUp, user } = useStoreWithInitializer(
    ({ register }) => register,
    dispatchOnCall(initializeRegister())
  );

  const { localization } = useLocalization();
  const navigate = useNavigate();
  return (
    <div className='auth-page'>
      <ContainerPage>
        <div className='col-md-6 offset-md-3 col-xs-12'>
          <h1 className='text-xs-center'>{localization.register.pageHeader}</h1>
          <p className='text-xs-center'>
            <a href='/login'>{localization.register.loginOption}</a>
          </p>

          <GenericForm
            disabled={signingUp}
            formObject={user as unknown as Record<string, string>}
            submitButtonText={localization.register.submit}
            errors={errors}
            onChange={onUpdateField}
            onSubmit={onSignUp(user, navigate)}
            fields={[
              buildGenericFormField({ name: 'username', placeholder: localization.register.username }),
              buildGenericFormField({ name: 'email', placeholder: localization.register.email }),
              buildGenericFormField({
                name: 'password',
                placeholder: localization.register.password,
                type: 'password',
              }),
            ]}
          />
        </div>
      </ContainerPage>
    </div>
  );
}

function onUpdateField(name: string, value: string) {
  store.dispatch(updateField({ name: name as keyof RegisterState['user'], value }));
}

function onSignUp(user: UserForRegistration, navigate: NavigateFunction) {
  return async (ev: React.FormEvent) => {
    ev.preventDefault();
    store.dispatch(startSigningUp());
    const result = await signUp(user);

    result.match({
      err: (e) => store.dispatch(updateErrors(e)),
      ok: (user) => {
        navigate('/');
        loadUserIntoApp(user);
      },
    });
  };
}
