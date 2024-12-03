import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { CreateUserStoryGroupMutation, GetUserStoryGroupsQuery } from './cqrs';

interface Request {
  projectId: number;
}
interface RequestInternal extends Request {
  input: object;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useCreateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, RequestInternal>(CreateUserStoryGroupMutation, {
    ...options,
    variables: { projectId: options?.variables?.projectId ?? 0, input: {} },
    refetchQueries: [GetUserStoryGroupsQuery],
  });
