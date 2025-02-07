import { IPaged, IProjectBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { newPaginationParameter, PaginationParameter } from '../common';
import { GetProjectsQuery } from './cqrs';

type GetProjectsRequest = PaginationParameter;
export interface GetProjectsResponse {
  projects: IPaged<IProjectBaseDto>;
}

export const useProjects = (
  options?: QueryHookOptions<GetProjectsResponse, GetProjectsRequest>
) => {
  return useQuery<GetProjectsResponse, GetProjectsRequest>(GetProjectsQuery, {
    ...options,
    variables: newPaginationParameter(options?.variables),
  });
};
