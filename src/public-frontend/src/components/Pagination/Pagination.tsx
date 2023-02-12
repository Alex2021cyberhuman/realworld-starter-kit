import React from 'react';
import { range } from 'ramda';
import { formatString, useLocalization } from '../../services/localizations/localization';

export function Pagination({
  currentPage,
  count,
  itemsPerPage,
  onPageChange,
}: {
  currentPage: number;
  count: number;
  itemsPerPage: number;
  onPageChange?: (index: number) => void;
}) {
  const { localization } = useLocalization();
  return (
    <nav>
      <ul className='pagination'>
        {Math.ceil(count / itemsPerPage) > 1 &&
          range(1, Math.ceil(count / itemsPerPage) + 1).map((index) => (
            <li
              key={index}
              className={`page-item${currentPage !== index ? '' : ' active'}`}
              onClick={onPageChange && (() => onPageChange(index))}
            >
              <a
                className='page-link'
                aria-label={formatString(localization.pagination.goToPage, index).toString()}
                href='#'
              >
                {index}
              </a>
            </li>
          ))}
      </ul>
    </nav>
  );
}
