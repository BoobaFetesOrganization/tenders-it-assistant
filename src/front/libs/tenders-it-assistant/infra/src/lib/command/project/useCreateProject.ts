import { MutationHookOptions, useMutation } from '@apollo/client';
import { IProjectDto } from '@tenders-it-assistant/domain';
import { CreateProjectMutation, GetProjectsQuery } from './cqrs';

interface Request {
  input: { name: string };
}
interface Response {
  project: IProjectDto;
}

export const useCreateProject = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(CreateProjectMutation, {
    ...options,
    refetchQueries: [GetProjectsQuery],
  });
