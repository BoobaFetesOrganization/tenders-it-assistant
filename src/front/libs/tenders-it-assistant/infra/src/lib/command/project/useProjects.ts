import { QueryHookOptions, useQuery } from '@apollo/client';
import { IPaged, IProjectBaseDto } from '@tenders-it-assistant/domain';
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
