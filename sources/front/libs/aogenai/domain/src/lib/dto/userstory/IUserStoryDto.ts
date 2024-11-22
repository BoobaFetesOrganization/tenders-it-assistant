import { IUserStoryBaseDto, newUserStoryBaseDto } from './IUserStoryBaseDto';
import { IUserStoryTaskDto } from './IUserStoryTaskDto';

export interface IUserStoryDto extends IUserStoryBaseDto {
  tasks: IUserStoryTaskDto[];
}

export function newUserStoryDto(obj?: Partial<IUserStoryDto>): IUserStoryDto {
  return {
    ...newUserStoryBaseDto(obj),
    tasks: obj?.tasks || [],
  };
}
