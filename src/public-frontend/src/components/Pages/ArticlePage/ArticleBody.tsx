import MDEditor from '@uiw/react-md-editor';

export function ArticleBody({ body }: { body: string }) {
  return (
    <div className='col-md-12' data-color-mode='light'>
      <MDEditor.Markdown source={body} linkTarget='_blank' />
    </div>
  );
}
