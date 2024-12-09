import { IUserStoryGroupDto } from '@aogenai/domain';
import {
  MutationHookOptions,
  MutationTuple,
  useMutation,
} from '@apollo/client';
import { GetProjectQuery } from '../cqrs';
import { GetUserStoryGroupQuery, ValidateUserStoryGroupMutation } from './cqrs';

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

export const useValidateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, RequestInternal>(ValidateUserStoryGroupMutation, {
    ...options,
    variables: {
      projectId: options?.variables?.projectId ?? 0,
      id: options?.variables?.id ?? 0,
      input: {},
    },
    refetchQueries: [
      GetUserStoryGroupQuery,
      GetUserStoryGroupQuery,
      GetProjectQuery,
    ],
  }) as unknown as MutationTuple<Response, Request>;
