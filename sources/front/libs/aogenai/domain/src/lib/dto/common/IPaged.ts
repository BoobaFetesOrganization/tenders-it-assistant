export interface IPaged<T> {
  page: IPageOptions;
  data: T[];
}

export interface IPageOptions {
  offset: number;
  limit: number;
  count?: number;
}

export function newPage<T>(obj?: Partial<IPaged<T>>): IPaged<T> {
  return {
    page: newPageOptions(obj?.page),
    data: obj?.data ?? [],
  };
}

export function newPageOptions(obj?: Partial<IPageOptions>): IPageOptions {
  return {
    offset: obj?.offset ?? 0,
    limit: obj?.limit ?? 10,
    count: obj?.count,
  };
}
