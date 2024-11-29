import { IEntityBaseDto, newEntityBaseDto } from '../../common';
import { IUserStoryBaseDto } from './userstory';

export interface IUserStoryGroupBaseDto extends IEntityBaseDto {
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
