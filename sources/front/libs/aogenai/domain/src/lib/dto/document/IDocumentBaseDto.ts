import { IEntityDomain } from '../common';

export interface IDocumentBaseDto extends IEntityDomain {
  name: string;
}

export function newDocumentBaseDto(
  obj?: Partial<IDocumentBaseDto>
): IDocumentBaseDto {
  return {
    id: obj?.id || 0,
    name: obj?.name || '',
  };
}
