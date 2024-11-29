import { IUserStoryBaseDto, newUserStoryBaseDto } from './IUserStoryBaseDto';
import { ITaskDto } from './task';

export interface IUserStoryDto extends IUserStoryBaseDto {
  cost: number;
  tasks: ITaskDto[];
}

export function newUserStoryDto(obj?: Partial<IUserStoryDto>): IUserStoryDto {
  return {
    ...newUserStoryBaseDto(obj),
    cost: obj?.cost || 0,
    tasks: obj?.tasks || [],
  };
}
