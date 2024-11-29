import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { CreateUserStoryGroupMutation, GetUserStoryGroupQuery } from './cqrs';

interface Request {
  projectId: number;
  input: IUserStoryGroupDto;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useCreateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(CreateUserStoryGroupMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
