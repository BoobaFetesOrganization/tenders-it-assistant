import { IProjectBaseDto, IProjectDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
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
