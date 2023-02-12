import React from 'react';
import { render, screen } from '@testing-library/react';
import { ListFormGroup } from './ListFormGroup';

it('Input should be large', () => {
  render(
    <ListFormGroup
      type='text'
      placeholder='1234'
      onChange={() => {}}
      onEnter={() => {}}
      lg
      listValue={[]}
      disabled
      value=''
      onRemoveItem={() => {}}
    />
  );

  expect(screen.getByPlaceholderText('1234').className.split(' ')).toContain('form-control-lg');
});
