import { IEntityDomain, newEntityDomain } from '../../common';
import {
  IUserStoryPromptDto,
  newUserStoryPromptDto,
} from './IUserStoryPromptDto';
import { IUserStoryDto } from './userstory';

export interface IUserStoryGroupDto extends IEntityDomain {
  projectId: string;
  request: IUserStoryPromptDto;
  response?: string;
  userStories: IUserStoryDto[];
}

export function newUserStoryGroupDto(
  obj?: Partial<IUserStoryGroupDto>
): IUserStoryGroupDto {
  return {
    ...newEntityDomain(obj),
    projectId: obj?.projectId || '',
    request: newUserStoryPromptDto(obj?.request),
    response: undefined,
    userStories: obj?.userStories || [],
  };
}
