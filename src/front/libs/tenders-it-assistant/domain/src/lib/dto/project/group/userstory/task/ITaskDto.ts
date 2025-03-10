import { ITaskBaseDto, newTaskBaseDto } from './ITaskBaseDto';
import { ITaskCostDto } from './cost';

export interface ITaskDto extends ITaskBaseDto {
  workingCosts: ITaskCostDto[];
}

export function newTaskDto(obj?: Partial<ITaskDto>): ITaskDto {
  return {
    ...newTaskBaseDto(obj),
    workingCosts: obj?.workingCosts || [],
  };
}
