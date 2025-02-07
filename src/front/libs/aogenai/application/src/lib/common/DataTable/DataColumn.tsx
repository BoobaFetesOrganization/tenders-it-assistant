import { JSX, memo } from 'react';
import { IDataColumnProps } from './IDataColumnProps';

export const DataColumn = memo(<T,>(props: IDataColumnProps<T>) => null) as <T>(
  props: IDataColumnProps<T>
) => JSX.Element;

export const ColumnName = 'DataColumn';
// eslint-disable-next-line @typescript-eslint/no-explicit-any
(DataColumn as any).displayName = ColumnName;
