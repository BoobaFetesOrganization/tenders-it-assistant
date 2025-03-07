import { IEntityBaseDto, newEntityBaseDto } from '../../../../../common';
import { TaskCostKind } from './TaskCostKind';

export interface ITaskCostDto extends IEntityBaseDto {
  kind: TaskCostKind;
  cost: number;
}

export function newTaskCostDto(obj?: Partial<ITaskCostDto>): ITaskCostDto {
  return {
    ...newEntityBaseDto(obj),
    kind: obj?.kind || TaskCostKind.Human,
    cost: obj?.cost || 0,
  };
}
