import { IUserStoryDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { DeleteUserStoryMutation, GetUserStoriesQuery } from './cqrs';

interface DeleteUserStoryRequest {
  projectId: number;
  groupId: number;
  id: number;
}
interface DeleteUserStoryResponse {
  userstory: IUserStoryDto;
}

export const useDeleteUserStory = (
  options?: MutationHookOptions<DeleteUserStoryResponse, DeleteUserStoryRequest>
) => {
  return useMutation<DeleteUserStoryResponse, DeleteUserStoryRequest>(
    DeleteUserStoryMutation,
    {
      ...options,
      refetchQueries: [GetUserStoriesQuery],
    }
  );
};
