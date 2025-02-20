import { IEntityBaseDto, newEntityBaseDto } from '../../../../common';

export interface ITaskBaseDto extends IEntityBaseDto {
  userStoryId: string;
  cost: number;
}

export function newTaskBaseDto(obj?: Partial<ITaskBaseDto>): ITaskBaseDto {
  return {
    ...newEntityBaseDto(obj),
    userStoryId: obj?.userStoryId || '',
    cost: obj?.cost || 0,
  };
}
