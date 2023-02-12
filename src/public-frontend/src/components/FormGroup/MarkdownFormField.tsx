import MDEditor from '@uiw/react-md-editor';
import React from 'react';
import rehypeSanitize from 'rehype-sanitize';
import { useLocalization } from '../../services/localizations/localization';
import { getCommands } from './getCommands';
import { getExtraCommands } from './getExtraCommands';

export interface ArticleImage {
  id: string;
  articleId?: string | undefined;
  url: string;
  mediaType: string;
}

export function MarkdownFormFieldWithImages({
  disabled,
  value,
  onChange,
  placeholder,
  articleImages,
  onUpload,
  onRemove,
}: {
  articleImages: ArticleImage[];
  onUpload: (image: File) => void;
  onRemove: (id: string) => void;
  placeholder: string;
  disabled: boolean;
  value: string;
  onChange: (value?: string | undefined) => void;
}) {
  const { localization } = useLocalization();

  function handleDropAndPaste(files?: File[] | undefined) {
    if (files && files.length > 0) {
      files.filter((file) => file.type.startsWith('image/')).forEach((file) => onUpload(file));
    }
  }

  function onChangeIfNotDisabled(value: string | undefined): void {
    if (!disabled) {
      onChange(value);
    }
  }

  return (
    <fieldset className='form-group'>
      <div className='form-control' data-color-mode='light'>
        <MDEditor
          value={value}
          placeholder={placeholder}
          onPaste={(e) => handleDropAndPaste([...e.clipboardData.files])}
          onDrop={(e) => handleDropAndPaste([...e.dataTransfer.files])}
          height={getHeight(value)}
          onChange={(value) => onChangeIfNotDisabled(value)}
          commands={getCommands()}
          extraCommands={getExtraCommands()}
          previewOptions={{ rehypePlugins: [[rehypeSanitize]] }}
        />
        <div className='card'>
          <input
            type='file'
            name='images'
            accept='image/*'
            onChange={(e) => (e.target.files ? handleDropAndPaste([...e.target.files]) : {})}
          />
          <ul className='list-group list-group-flush'>
            {articleImages.map((image) => (
              <li key={image.id} className='list-group-item'>
                <img src={image.url} alt={image.articleId} />
                <button
                  className='btn btn-outline-primary'
                  onClick={() => {
                    onRemove(image.id);
                  }}
                >
                  {localization.articleEditor.imageRemove}
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </fieldset>
  );
}

function getHeight(value: string): number | undefined {
  return Math.max(300, value.split('\n').length * 30);
}
