import axios from 'axios';
import React from 'react';
import { updateSettings } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { useStore } from '../../../state/storeHooks';
import { UserSettings } from '../../../types/user';
import { buildGenericFormField } from '../../../types/genericFormField';
import { loadUser, logout } from '../../App/App.slice';
import { GenericForm } from '../../GenericForm/GenericForm';
import { SettingsState, startUpdate, updateErrors, updateField } from './Settings.slice';
import { ContainerPage } from '../../ContainerPage/ContainerPage';
import { useLocalization } from '../../../services/localizations/localization';
import { NavigateFunction, useNavigate } from 'react-router-dom';

export interface SettingsField {
  name: keyof UserSettings;
  type?: string;
  isTextArea?: true;
  placeholder: string;
}

export function Settings() {
  const { user, errors, updating } = useStore(({ settings }) => settings);
  const { localization } = useLocalization();
  const navigate = useNavigate();
  return (
    <div className='settings-page'>
      <ContainerPage>
        <div className='col-md-6 offset-md-3 col-xs-12'>
          <h1 className='text-xs-center'>{localization.settings.pageHeader}</h1>

          <GenericForm
            disabled={updating}
            formObject={{ ...user }}
            submitButtonText={localization.settings.updateSettings}
            errors={errors}
            onChange={onUpdateField}
            onSubmit={onUpdateSettings(user, navigate)}
            fields={[
              buildGenericFormField({
                name: 'image',
                placeholder: localization.settings.image,
              }),
              buildGenericFormField({
                name: 'username',
                placeholder: localization.settings.username,
              }),
              buildGenericFormField({
                name: 'bio',
                placeholder: localization.settings.bio,
                rows: 8,
                fieldType: 'textarea',
              }),
              buildGenericFormField({
                name: 'email',
                placeholder: localization.settings.email,
              }),
              buildGenericFormField({
                name: 'password',
                placeholder: localization.settings.password,
                type: 'password',
              }),
            ]}
          />

          <hr />
          <button className='btn btn-outline-danger' onClick={() => _logout(navigate)}>
            {localization.settings.logout}
          </button>
        </div>
      </ContainerPage>
    </div>
  );
}

function onUpdateField(name: string, value: string) {
  store.dispatch(updateField({ name: name as keyof SettingsState['user'], value }));
}

function onUpdateSettings(user: UserSettings, navigate: NavigateFunction) {
  return async (ev: React.FormEvent) => {
    ev.preventDefault();
    store.dispatch(startUpdate());
    const result = await updateSettings(user);

    result.match({
      err: (e) => store.dispatch(updateErrors(e)),
      ok: (user) => {
        store.dispatch(loadUser(user));
        navigate('/');
      },
    });
  };
}

function _logout(navigate: NavigateFunction) {
  delete axios.defaults.headers.common.Authorization;
  localStorage.removeItem('token');
  store.dispatch(logout());
  navigate('/');
}
