import { IEntityBaseDto, newEntityBaseDto } from '../../../../common';

export interface ITaskBaseDto extends IEntityBaseDto {
  userStoryId: number;
  cost: number;
}

export function newTaskBaseDto(obj?: Partial<ITaskBaseDto>): ITaskBaseDto {
  return {
    ...newEntityBaseDto(obj),
    userStoryId: obj?.userStoryId || 0,
    cost: obj?.cost || 0,
  };
}
