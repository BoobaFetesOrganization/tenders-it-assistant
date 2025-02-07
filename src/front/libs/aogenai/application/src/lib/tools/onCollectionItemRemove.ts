interface ISetCollectionItemArgs<T extends object> {
  collection: T[];
  setCollection: (value: T[]) => void;
  item: T;
}

export function onCollectionItemRemove<T extends object>({
  collection,
  item,
  setCollection,
}: ISetCollectionItemArgs<T>) {
  return () => {
    setCollection(collection.filter((i) => i !== item));
  };
}
