import { ITaskBaseDto, newTaskBaseDto } from './ITaskBaseDto';
import { ITaskCostDto } from './cost';

export interface ITaskDto extends ITaskBaseDto {
  WorkingCosts: ITaskCostDto[];
}

export function newTaskDto(obj?: Partial<ITaskDto>): ITaskDto {
  return {
    ...newTaskBaseDto(obj),
    WorkingCosts: obj?.WorkingCosts || [],
  };
}
