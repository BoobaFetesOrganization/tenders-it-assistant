import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { GetUserStoryGroupQuery } from './cqrs';
import { UpdateUserStoryGroupRequestMutation } from './cqrs/UpdateUserStoryGroupRequestMutation';

interface Request {
  projectId: number;
  input: IUserStoryGroupDto;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useUpdateUserStoryGroupRequest = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateUserStoryGroupRequestMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
