import { ChangeEvent } from 'react';

interface ISetPropertyArgs<T extends object> {
  data: T;
  setData: (value: T) => void;
  property: keyof T;
  getValue?: (value: string) => T[keyof T];
}
function newArgs<T extends object>(args: ISetPropertyArgs<T>) {
  return {
    ...args,
    getValue: args.getValue || ((value) => value),
  };
}
export function onPropertyChange<T extends object>(args: ISetPropertyArgs<T>) {
  const { property, data, setData, getValue } = newArgs(args);
  return (e: ChangeEvent<HTMLInputElement>) => {
    setData({ ...data, [property]: getValue(e.target.value) });
  };
}
