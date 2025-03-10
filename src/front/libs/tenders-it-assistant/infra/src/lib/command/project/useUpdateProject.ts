import { MutationHookOptions, useMutation } from '@apollo/client';
import { IProjectBaseDto, IProjectDto } from '@tenders-it-assistant/domain';
import { GetProjectQuery, UpdateProjectMutation } from './cqrs';

interface Request {
  input: IProjectBaseDto;
}
interface Response {
  project: IProjectDto;
}

export const useUpdateProject = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateProjectMutation, {
    ...options,
    refetchQueries: [GetProjectQuery, GetProjectQuery],
  });
