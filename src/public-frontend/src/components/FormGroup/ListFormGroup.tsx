import React from 'react';
import { onListFieldKeyUp } from './onListFieldKeyUp';

export function ListFormGroup({
  type,
  placeholder,
  disabled,
  value,
  listValue,
  lg,
  onChange,
  onEnter,
  onRemoveItem,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  value: string;
  listValue: string[];
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLInputElement>) => void;
  onEnter: () => void;
  onRemoveItem: (index: number) => void;
}) {
  return (
    <fieldset className='form-group'>
      <input
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange }}
        onKeyDown={(ev) => ev.key === 'Enter' && ev.preventDefault()}
        onKeyUp={onListFieldKeyUp(onEnter)}
      />
      <div className='tag-list'>
        {listValue.map((value, index) => (
          <span key={value} className='tag-default tag-pill' onClick={() => onRemoveItem(index)}>
            <i className='ion-close-round'></i>
            {value}
          </span>
        ))}
      </div>
    </fieldset>
  );
}
