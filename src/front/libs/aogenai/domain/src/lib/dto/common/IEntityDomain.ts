export interface IEntityDomain {
  id: number;
}

export function newEntityDomain(obj?: Partial<IEntityDomain>): IEntityDomain {
  return {
    id: obj?.id || 0,
  };
}

export interface IEntityBaseDto extends IEntityDomain {
  name: string;
}

export function newEntityBaseDto(
  obj?: Partial<IEntityBaseDto>
): IEntityBaseDto {
  return {
    ...newEntityDomain(obj),
    name: obj?.name || '',
  };
}
