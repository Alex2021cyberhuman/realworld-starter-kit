import React from 'react';

export function FormGroup({
  type,
  placeholder,
  disabled,
  value,
  onChange,
  lg,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  value: string;
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLInputElement>) => void;
}) {
  return (
    <fieldset className='form-group'>
      <input
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange }}
      />
    </fieldset>
  );
}
