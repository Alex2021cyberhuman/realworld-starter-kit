import React from 'react';

export function Errors({ errors }: { errors: string[] }) {
  return (
    <ul className='error-messages'>
      {errors.map((error, index) => (
        <li key={`error-${index}`}>{error}</li>
      ))}
    </ul>
  );
}
