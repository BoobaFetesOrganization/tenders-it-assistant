import { IEntityDomain } from '../common';

export interface IProjectBaseDto extends IEntityDomain {
  name: string;
}

export function newProjectBaseDto(
  obj?: Partial<IProjectBaseDto>
): IProjectBaseDto {
  return {
    id: obj?.id || 0,
    name: obj?.name || '',
  };
}
