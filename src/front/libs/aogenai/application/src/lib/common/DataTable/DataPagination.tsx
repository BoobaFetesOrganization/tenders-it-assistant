import { FC, memo, PropsWithChildren } from 'react';
import { IDataPaginationProps } from './IDataPaginationProps';

export const DataPagination: FC<PropsWithChildren<IDataPaginationProps>> = memo(
  () => null
);

export const PaginationName = 'Pagination';
// eslint-disable-next-line @typescript-eslint/no-explicit-any
(DataPagination as any).displayName = PaginationName;
