import { IProjectDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { GetProjectQuery } from './cqrs';

interface Request {
  id: number;
}
interface Response {
  project: IProjectDto;
}

export const useProject = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetProjectQuery, {
    ...options,
    skip: !options?.variables?.id || options?.skip,
  });
