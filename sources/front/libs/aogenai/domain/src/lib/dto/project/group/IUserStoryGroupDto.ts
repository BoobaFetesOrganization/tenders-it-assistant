import { IEntityBaseDto, newEntityBaseDto } from '../../common';
import {
  IUserStoryPromptDto,
  newUserStoryPromptDto,
} from './IUserStoryPromptDto';
import { IUserStoryBaseDto } from './userstory';

export interface IUserStoryGroupDto extends IEntityBaseDto {
  projectId: number;
  request: IUserStoryPromptDto;
  response?: string;
  userStories: IUserStoryBaseDto[];
}

export function newUserStoryGroupDto(
  obj?: Partial<IUserStoryGroupDto>
): IUserStoryGroupDto {
  return {
    ...newEntityBaseDto(obj),
    projectId: obj?.projectId || 0,
    request: newUserStoryPromptDto(obj?.request),
    response: undefined,
    userStories: obj?.userStories || [],
  };
}
