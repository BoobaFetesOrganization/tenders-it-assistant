import { IEntityDomain, newEntityBaseDto } from '../../common';
import { IUserStoryBaseDto } from './userstory';

export interface IUserStoryGroupBaseDto extends IEntityDomain {
  projectId: number;
  userStories: IUserStoryBaseDto[];
}

export function newUserStoryGroupBaseDto(
  obj?: Partial<IUserStoryGroupBaseDto>
): IUserStoryGroupBaseDto {
  return {
    ...newEntityBaseDto(obj),
    projectId: obj?.projectId || 0,
    userStories: obj?.userStories || [],
  };
}
