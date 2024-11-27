import { IPaged, IProjectBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { getInfraSettings } from '../../settings';
import { GetProjectsQuery } from './cqrs';

interface GetProjectsRequest {
  limit: number;
  offset: number;
}
export interface GetProjectsResponse {
  projects: IPaged<IProjectBaseDto>;
}

export const useProjects = (
  options?: QueryHookOptions<GetProjectsResponse, GetProjectsRequest>
) => {
  const maxLimit = getInfraSettings().api.maxLimit;

  return useQuery<GetProjectsResponse, GetProjectsRequest>(GetProjectsQuery, {
    ...options,
    variables: { offset: 0, limit: maxLimit, ...options?.variables },
  });
};
