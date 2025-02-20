import { IUserStoryGroupDto } from '@aogenai/domain';
import {
  MutationHookOptions,
  MutationTuple,
  useMutation,
} from '@apollo/client';
import { CreateUserStoryGroupMutation, GetUserStoryGroupsQuery } from './cqrs';

interface Request {
  projectId: string;
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
    variables: { projectId: options?.variables?.projectId ?? '', input: {} },
    refetchQueries: [GetUserStoryGroupsQuery],
  }) as unknown as MutationTuple<Response, Request>;
