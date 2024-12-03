import { IUserStoryBaseDto, newUserStoryBaseDto } from './IUserStoryBaseDto';
import { ITaskDto } from './task';

export interface IUserStoryDto extends IUserStoryBaseDto {
  tasks: ITaskDto[];
}

export function newUserStoryDto(obj?: Partial<IUserStoryDto>): IUserStoryDto {
  return {
    ...newUserStoryBaseDto(obj),
    tasks: obj?.tasks || [],
  };
}
