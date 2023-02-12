import React from 'react';
import { createComment, getArticleComments } from '../../../services/webapi/conduit';
import { store } from '../../../state/store';
import { loadComments, startSubmittingComment } from './ArticlePage.slice';

export function onPostComment(slug: string, body: string): (ev: React.FormEvent) => void {
  return async (ev) => {
    ev.preventDefault();

    store.dispatch(startSubmittingComment());
    await createComment(slug, body);

    store.dispatch(loadComments(await getArticleComments(slug)));
  };
}
