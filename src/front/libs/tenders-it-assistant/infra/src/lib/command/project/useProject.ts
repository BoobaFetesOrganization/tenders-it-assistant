import { QueryHookOptions, useQuery } from '@apollo/client';
import { IProjectDto } from '@tenders-it-assistant/domain';
import { GetProjectQuery } from './cqrs';

interface Request {
  id: string;
}
interface Response {
  project: IProjectDto;
}

export const useProject = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetProjectQuery, {
    ...options,
    skip: !options?.variables?.id || options?.skip,
  });
