export interface IEntityDomain {
  id: string;
  timestamp?: Date;
}

export function newEntityDomain(obj?: Partial<IEntityDomain>): IEntityDomain {
  return {
    id: obj?.id || '',
    timestamp: obj?.timestamp || undefined,
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
