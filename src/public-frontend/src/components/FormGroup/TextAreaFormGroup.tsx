import React from 'react';

export function TextAreaFormGroup({
  type,
  placeholder,
  disabled,
  value,
  rows,
  onChange,
  lg,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  rows: number;
  value: string;
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLTextAreaElement>) => void;
}) {
  return (
    <fieldset className='form-group'>
      <textarea
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange, rows }}
      ></textarea>
    </fieldset>
  );
}
