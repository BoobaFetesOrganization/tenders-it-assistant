interface ISetCollectionItemArgs<T extends object> {
  collection: T[];
  setCollection: (value: T[]) => void;
  item: T;
}

export function onCollectionItemChange<T extends object>({
  collection,
  item,
  setCollection,
}: ISetCollectionItemArgs<T>) {
  return (newItem: T) => {
    setCollection(collection.map((i) => (i === item ? newItem : i)));
  };
}
