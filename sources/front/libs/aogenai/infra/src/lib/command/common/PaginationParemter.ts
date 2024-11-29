import { getInfraSettings } from '../../settings';

export interface PaginationParameter {
  limit: number;
  offset: number;
}

export function newPaginationParameter(
  obj?: Partial<PaginationParameter>
): PaginationParameter {
  const maxLimit = getInfraSettings().api.maxLimit;
  return {
    limit: obj?.limit || maxLimit,
    offset: obj?.offset || 0,
  };
}
