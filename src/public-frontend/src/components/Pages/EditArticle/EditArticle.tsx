import React, { Fragment, useEffect } from 'react';
import { NavigateFunction, useNavigate, useParams } from 'react-router-dom';
import { getArticle, updateArticle } from '../../../services/webapi/conduit';
import {
  assingImagesToArticle,
  getArticleImages,
  removeArticleImage,
  uploadArticleImage,
} from '../../../services/webapi/images';
import { store } from '../../../state/store';
import { useStore } from '../../../state/storeHooks';
import { ArticleEditor } from '../../ArticleEditor/ArticleEditor';
import {
  initializeEditor,
  loadArticle,
  loadImages,
  startSubmitting,
  updateErrors,
} from '../../ArticleEditor/ArticleEditor.slice';

export function EditArticle() {
  const { slug } = useParams<{ slug: string }>();
  if (!slug) {
    throw 'slug is undefined';
  }
  const { loading } = useStore(({ editor }) => editor);
  const navigate = useNavigate();
  useEffect(() => {
    if (slug) {
      _loadArticle(slug, navigate);
    }
    // noinspection JSIgnoredPromiseFromCall
  }, [slug]);

  return (
    <Fragment>
      {!loading && (
        <ArticleEditor
          onSubmit={onSubmit(slug, navigate)}
          onUploadImageAsync={uploadArticleImage}
          onRemoveImageAsync={removeArticleImage}
        />
      )}
    </Fragment>
  );
}

async function _loadArticle(slug: string, navigate: NavigateFunction) {
  store.dispatch(initializeEditor());
  try {
    const { title, description, body, tagList, author, id } = await getArticle(slug);
    const images = await getArticleImages(id);
    if (author.username !== store.getState().app.user.unwrap().username) {
      navigate('/');
      return;
    }

    store.dispatch(loadImages(images));
    store.dispatch(loadArticle({ title, description, body, tagList }));
  } catch {
    navigate('/');
  }
}

function onSubmit(slug: string, navigate: NavigateFunction): (ev: React.FormEvent) => void {
  return async (ev) => {
    ev.preventDefault();

    store.dispatch(startSubmitting());
    const editorStore = store.getState().editor;
    const article = editorStore.article;
    const imageIds = editorStore.articleImages.map((x) => x.id);
    const result = await updateArticle(slug, article);
    if (result.isOk()) {
      const articleResponse = result.ok().unwrap();
      const id = articleResponse.id;
      await assingImagesToArticle(id, imageIds);
      navigate(`/article/${slug}`);
    } else if (result.isErr()) {
      store.dispatch(updateErrors(result.err().unwrap()));
    }
  };
}
