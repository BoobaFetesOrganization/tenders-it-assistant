import { PaginationProps } from '@mui/material';
import { PropsWithChildren } from 'react';

export interface IDataPaginationProps
  extends PropsWithChildren,
    Pick<PaginationProps, 'count' | 'disabled' | 'onChange'> {
  maxItemPerPage: number;
}
