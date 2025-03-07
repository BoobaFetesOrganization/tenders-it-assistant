import { FC } from 'react';

export interface IDataColumnProps<T> {
  title?: string;
  source?: keyof T;
  value?(item: T): string;
  actions?: FC<T>;
  fill?: boolean;
  onSelect?(item: T): void;
}
