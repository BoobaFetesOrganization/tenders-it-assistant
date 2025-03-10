import { MutationHookOptions, useMutation } from '@apollo/client';
import { IUserStoryGroupDto } from '@tenders-it-assistant/domain';
import { DeleteUserStoryGroupMutation, GetUserStoryGroupsQuery } from './cqrs';

interface DeleteUserStoryGroupRequest {
  projectId: string;
  id: string;
}
interface DeleteUserStoryGroupResponse {
  group: IUserStoryGroupDto;
}

export const useDeleteUserStoryGroup = (
  options?: MutationHookOptions<
    DeleteUserStoryGroupResponse,
    DeleteUserStoryGroupRequest
  >
) => {
  return useMutation<DeleteUserStoryGroupResponse, DeleteUserStoryGroupRequest>(
    DeleteUserStoryGroupMutation,
    {
      ...options,
      refetchQueries: [GetUserStoryGroupsQuery],
    }
  );
};
