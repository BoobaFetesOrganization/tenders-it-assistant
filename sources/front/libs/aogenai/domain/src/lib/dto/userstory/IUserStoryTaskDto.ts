import { IEntityDomain } from '../common';

export interface IUserStoryTaskDto extends IEntityDomain {
  name: string;
  cost: number;
}

export function newUserStoryaskDto(
  obj?: Partial<IUserStoryTaskDto>
): IUserStoryTaskDto {
  return {
    id: obj?.id || 0,
    name: obj?.name || '',
    cost: obj?.cost || 0,
  };
}
