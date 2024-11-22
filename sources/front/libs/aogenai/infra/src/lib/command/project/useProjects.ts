import { IProjectBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { GetProjectsQuery } from './cqrs';

// eslint-disable-next-line @typescript-eslint/no-empty-interface, @typescript-eslint/no-empty-object-type
interface Request {}
interface Response {
  projects: IProjectBaseDto[];
}

export const useProjects = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetProjectsQuery, options);
