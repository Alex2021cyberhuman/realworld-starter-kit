import React, { FormEvent, useEffect } from 'react';
import { NavigateFunction, useNavigate } from 'react-router-dom';
import { createArticle } from '../../../services/webapi/conduit';
import { uploadArticleImage, removeArticleImage, assingImagesToArticle } from '../../../services/webapi/images';
import { store } from '../../../state/store';
import { ArticleEditor } from '../../ArticleEditor/ArticleEditor';
import { initializeEditor, startSubmitting, updateErrors } from '../../ArticleEditor/ArticleEditor.slice';

export function NewArticle() {
  useEffect(() => {
    store.dispatch(initializeEditor());
  }, [null]);
  const navigate = useNavigate();

  return (
    <ArticleEditor
      onSubmit={(ev: FormEvent) => onSubmit(ev, navigate)}
      onUploadImageAsync={uploadArticleImage}
      onRemoveImageAsync={removeArticleImage}
    />
  );
}

async function onSubmit(ev: FormEvent, navigate: NavigateFunction) {
  ev.preventDefault();
  store.dispatch(startSubmitting());

  const editorStore = store.getState().editor;
  const article = editorStore.article;
  const imageIds = editorStore.articleImages.map((x) => x.id);
  const result = await createArticle(article);
  if (result.isOk()) {
    const articleResponse = result.ok().unwrap();
    const slug = articleResponse.slug;
    const id = articleResponse.id;
    await assingImagesToArticle(id, imageIds);
    navigate(`/article/${slug}`);
  } else if (result.isErr()) {
    store.dispatch(updateErrors(result.err().unwrap()));
  }
}
