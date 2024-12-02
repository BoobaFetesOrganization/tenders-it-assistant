interface ISetCollectionAddArgs<T extends object> {
  collection: T[];
  setCollection: (value: T[]) => void;
}

export function onCollectionAdd<T extends object>({
  collection,
  setCollection,
}: ISetCollectionAddArgs<T>) {
  return (newItem: T) => {
    setCollection([...collection, newItem]);
  };
}
