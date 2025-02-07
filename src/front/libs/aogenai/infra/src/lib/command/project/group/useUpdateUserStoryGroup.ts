import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { GetUserStoryGroupQuery, UpdateUserStoryGroupMutation } from './cqrs';

interface Request {
  projectId: number;
  input: IUserStoryGroupDto;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useUpdateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateUserStoryGroupMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
