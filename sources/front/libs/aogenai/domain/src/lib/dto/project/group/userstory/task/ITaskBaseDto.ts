import { IEntityBaseDto, newEntityBaseDto } from '../../../../common';

export interface ITaskBaseDto extends IEntityBaseDto {
  cost: number;
}

export function newTaskBaseDto(obj?: Partial<ITaskBaseDto>): ITaskBaseDto {
  return {
    ...newEntityBaseDto(obj),
    cost: obj?.cost || 0,
  };
}
