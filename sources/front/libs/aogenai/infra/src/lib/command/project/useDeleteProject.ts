import { IProjectDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { DeleteProjectMutation, GetProjectsQuery } from './cqrs';

interface DeleteProjectRequest {
  id: number;
}
interface DeleteProjectResponse {
  project: IProjectDto;
}

export const useDeleteProject = (
  options?: MutationHookOptions<DeleteProjectResponse, DeleteProjectRequest>
) => {
  return useMutation<DeleteProjectResponse, DeleteProjectRequest>(
    DeleteProjectMutation,
    {
      ...options,
      refetchQueries: [GetProjectsQuery],
    }
  );
};
