import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { ArticleForEditor } from '../../types/article';
import * as R from 'ramda';
import { GenericErrors } from '../../types/error';
import { ArticleImage } from '../FormGroup/MarkdownFormField';

export interface EditorState {
  article: ArticleForEditor;
  tag: string;
  submitting: boolean;
  errors: GenericErrors;
  loading: boolean;
  articleImages: ArticleImage[];
}

const initialState: EditorState = {
  article: { title: '', body: '', tagList: [], description: '' },
  tag: '',
  submitting: false,
  errors: new Map<string, string[]>(),
  loading: true,
  articleImages: [],
};

const slice = createSlice({
  name: 'editor',
  initialState,
  reducers: {
    initializeEditor: () => initialState,
    updateField: (
      state,
      { payload: { name, value } }: PayloadAction<{ name: keyof EditorState['article'] | 'tag'; value: string }>
    ) => {
      if (name === 'tag') {
        state.tag = value;
        return;
      }

      if (name !== 'tagList') {
        state.article[name] = value;
      }
    },
    updateErrors: (state, { payload: errors }: PayloadAction<GenericErrors>) => {
      state.errors = errors;
      state.submitting = false;
    },
    startSubmitting: (state) => {
      state.submitting = true;
    },
    addTag: (state) => {
      if (state.tag.length > 0) {
        state.article.tagList.push(state.tag);
        state.tag = '';
      }
    },
    removeTag: (state, { payload: index }: PayloadAction<number>) => {
      state.article.tagList = R.remove(index, 1, state.article.tagList);
    },
    afterUploadImage: (state, { payload }: PayloadAction<ArticleImage>) => {
      state.submitting = false;
      state.articleImages = [payload].concat(state.articleImages);
      const bodyWithImage = state.article.body + '\n' + `![](${payload.url})` + '\n';
      state.article = { ...state.article, body: bodyWithImage };
    },
    beforeUploadImage: (state) => {
      state.submitting = true;
    },
    afterRemoveImage: (state, { payload: id }: PayloadAction<string>) => {
      state.submitting = false;
      state.articleImages = state.articleImages.filter((v) => v.id !== id);
    },
    beforeRemoveImage: (state) => {
      state.submitting = true;
    },
    loadArticle: (state, { payload: article }: PayloadAction<ArticleForEditor>) => {
      state.article = article;
      state.submitting = false;
    },
    loadImages: (state, { payload: images }: PayloadAction<ArticleImage[]>) => {
      state.articleImages = images;
    },
  },
});

export const {
  initializeEditor,
  updateField,
  startSubmitting,
  addTag,
  removeTag,
  updateErrors,
  loadArticle,
  loadImages,
  afterUploadImage,
  beforeUploadImage,
  afterRemoveImage,
  beforeRemoveImage,
} = slice.actions;

export default slice.reducer;
