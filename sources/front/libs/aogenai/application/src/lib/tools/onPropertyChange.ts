import { ChangeEvent } from 'react';

interface ISetPropertyChangeArgs<T extends object> {
  property: keyof T;
  item: T;
  setItem: (value: T) => void;
  getValue?: (value: string) => T[keyof T];
}
function newArgs<T extends object>(args: ISetPropertyChangeArgs<T>) {
  return {
    ...args,
    getValue: args.getValue || ((value) => value),
  };
}
export function onPropertyChange<T extends object>(
  args: ISetPropertyChangeArgs<T>
) {
  const { property, item, setItem, getValue } = newArgs(args);
  return (e: ChangeEvent<HTMLInputElement>) => {
    setItem({ ...item, [property]: getValue(e.target.value) });
  };
}
