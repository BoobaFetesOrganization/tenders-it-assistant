import { IEntityDomain } from '../common';

export interface IUserStoryBaseDto extends IEntityDomain {
  name: string;
  cost: number;
}

export function newUserStoryBaseDto(
  obj?: Partial<IUserStoryBaseDto>
): IUserStoryBaseDto {
  return {
    id: obj?.id || 0,
    name: obj?.name || '',
    cost: obj?.cost || 0,
  };
}
