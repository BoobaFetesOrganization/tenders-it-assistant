import { IEntityDomain, newEntityDomain } from '../../common';
import {
  IUserStoryRequestDto,
  newUserStoryRequestDto,
} from './IUserStoryRequestDto';
import { IUserStoryDto } from './userstory';

export interface IUserStoryGroupDto extends IEntityDomain {
  projectId: string;
  request: IUserStoryRequestDto;
  response?: string;
  userStories: IUserStoryDto[];
}

export function newUserStoryGroupDto(
  obj?: Partial<IUserStoryGroupDto>
): IUserStoryGroupDto {
  return {
    ...newEntityDomain(obj),
    projectId: obj?.projectId || '',
    request: newUserStoryRequestDto(obj?.request),
    response: undefined,
    userStories: obj?.userStories || [],
  };
}
