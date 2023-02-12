import React from 'react';
import { store } from '../../state/store';
import { useStore } from '../../state/storeHooks';
import { buildGenericFormField } from '../../types/genericFormField';
import { ContainerPage } from '../ContainerPage/ContainerPage';
import { GenericForm } from '../GenericForm/GenericForm';
import {
  addTag,
  afterRemoveImage,
  afterUploadImage,
  beforeRemoveImage,
  beforeUploadImage,
  EditorState,
  removeTag,
  updateField,
} from './ArticleEditor.slice';
import { useLocalization } from '../../services/localizations/localization';
import { ArticleImage, MarkdownFormFieldWithImages } from '../FormGroup/MarkdownFormField';

export function ArticleEditor({
  onSubmit,
  onUploadImageAsync,
  onRemoveImageAsync,
}: {
  onSubmit: (ev: React.FormEvent) => void;
  slug?: string | undefined;
  onUploadImageAsync: (image: File) => Promise<ArticleImage>;
  onRemoveImageAsync: (id: string) => Promise<void>;
}) {
  const { article, submitting, tag, errors, articleImages } = useStore(({ editor }) => editor);
  const { localization } = useLocalization();

  function onRemoveImage(id: string) {
    store.dispatch(beforeRemoveImage());
    onRemoveImageAsync(id).then(() => store.dispatch(afterRemoveImage(id)));
  }

  function onUploadImage(image: File) {
    store.dispatch(beforeUploadImage());
    onUploadImageAsync(image).then((img) => store.dispatch(afterUploadImage(img)));
  }

  return (
    <div className='editor-page'>
      <ContainerPage>
        <div className='col-md-10 offset-md-1 col-xs-12'>
          <GenericForm
            formObject={{ ...article, tag } as unknown as Record<string, string | null>}
            disabled={submitting}
            errors={errors}
            onChange={onUpdateField}
            onSubmit={onSubmit}
            submitButtonText={localization.articleEditor.submitButtonText}
            onAddItemToList={onAddTag}
            onRemoveListItem={onRemoveTag}
            fields={[
              buildGenericFormField({
                name: 'title',
                placeholder: localization.articleEditor.title,
              }),
              buildGenericFormField({
                name: 'description',
                placeholder: localization.articleEditor.description,
                lg: false,
              }),
              buildGenericFormField({
                name: 'body',
                customElement: () => (
                  <MarkdownFormFieldWithImages
                    articleImages={articleImages}
                    onUpload={onUploadImage}
                    onRemove={onRemoveImage}
                    placeholder={localization.articleEditor.body}
                    disabled={submitting}
                    value={article.body}
                    onChange={(v) => onUpdateField('body', v || '')}
                  />
                ),
              }),
              buildGenericFormField({
                name: 'tag',
                placeholder: localization.articleEditor.tag,
                listName: 'tagList',
                fieldType: 'list',
                lg: false,
              }),
            ]}
          />
        </div>
      </ContainerPage>
    </div>
  );
}

function onUpdateField(name: string, value: string) {
  store.dispatch(updateField({ name: name as keyof EditorState['article'], value }));
}

function onAddTag() {
  store.dispatch(addTag());
}

function onRemoveTag(_: string, index: number) {
  store.dispatch(removeTag(index));
}
