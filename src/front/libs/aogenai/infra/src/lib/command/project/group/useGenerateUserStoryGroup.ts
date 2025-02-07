import { IUserStoryGroupDto } from '@aogenai/domain';
import {
  MutationHookOptions,
  MutationTuple,
  useMutation,
} from '@apollo/client';
import { GenerateUserStoryGroupMutation, GetUserStoryGroupQuery } from './cqrs';

interface Request {
  projectId: number;
  id: number;
}
interface RequestInternal extends Request {
  input: object;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useGenerateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, RequestInternal>(GenerateUserStoryGroupMutation, {
    ...options,
    variables: {
      projectId: options?.variables?.projectId ?? 0,
      id: options?.variables?.id ?? 0,
      input: {},
    },
    refetchQueries: [GetUserStoryGroupQuery],
  }) as unknown as MutationTuple<Response, Request>;
