import React from 'react';
import { store } from '../../../state/store';
import { updateCommentBody } from './ArticlePage.slice';

export function onCommentChange(ev: React.ChangeEvent<HTMLTextAreaElement>) {
  store.dispatch(updateCommentBody(ev.target.value));
}
