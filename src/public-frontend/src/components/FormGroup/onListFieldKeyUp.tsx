import React from 'react';

export function onListFieldKeyUp(onEnter: () => void): (ev: React.KeyboardEvent) => void {
  return (ev) => {
    if (ev.key === 'Enter') {
      ev.preventDefault();
      onEnter();
    }
  };
}
