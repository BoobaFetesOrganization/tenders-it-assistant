import { IPaged, IProjectBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { getInfraSettings } from '../../settings';
import { GetProjectsQuery } from './cqrs';

interface Request {
  limit: number;
  offset: number;
}
interface Response {
  projects: IPaged<IProjectBaseDto>;
}

export const useProjects = (options?: QueryHookOptions<Response, Request>) => {
  const maxLimit = getInfraSettings().api.maxLimit;

  return useQuery<Response, Request>(GetProjectsQuery, {
    ...options,
    variables: { offset: 0, limit: maxLimit, ...options?.variables },
  });
};
