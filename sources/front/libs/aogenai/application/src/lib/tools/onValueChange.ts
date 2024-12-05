interface ISetOnValueChangeArgs<T extends object> {
  property: keyof T;
  item: T;
  setItem: (value: T) => void;
}

export function onValueChange<T extends object>({
  item,
  setItem,
  property,
}: ISetOnValueChangeArgs<T>) {
  return (value: T[typeof property]) => {
    setItem({ ...item, [property]: value });
  };
}
