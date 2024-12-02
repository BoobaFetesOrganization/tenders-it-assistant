import { IEntityDomain, newEntityBaseDto } from '../../common';
import { IUserStoryBaseDto } from './userstory';

export interface IUserStoryGroupBaseDto extends IEntityDomain {
  userStories: IUserStoryBaseDto[];
}

export function newUserStoryGroupBaseDto(
  obj?: Partial<IUserStoryGroupBaseDto>
): IUserStoryGroupBaseDto {
  return {
    ...newEntityBaseDto(obj),
    userStories: obj?.userStories || [],
  };
}
