import { MutationHookOptions, useMutation } from '@apollo/client';
import { IProjectDto } from '@tenders-it-assistant/domain';
import { DeleteProjectMutation, GetProjectsQuery } from './cqrs';

interface DeleteProjectRequest {
  id: string;
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
