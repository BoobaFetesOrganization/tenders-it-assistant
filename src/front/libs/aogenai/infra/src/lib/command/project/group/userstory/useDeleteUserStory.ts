import { IUserStoryDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { DeleteUserStoryMutation, GetUserStoriesQuery } from './cqrs';

interface DeleteUserStoryRequest {
  projectId: string;
  groupId: string;
  id: string;
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
