import { IEntityBaseDto, newEntityBaseDto } from '../../../common';
import { ITaskBaseDto } from './task';

export interface IUserStoryBaseDto extends IEntityBaseDto {
  cost: number;
  tasks: ITaskBaseDto[];
}

export function newUserStoryBaseDto(
  obj?: Partial<IUserStoryBaseDto>
): IUserStoryBaseDto {
  return {
    ...newEntityBaseDto(obj),
    cost: obj?.cost || 0,
    tasks: obj?.tasks || [],
  };
}
