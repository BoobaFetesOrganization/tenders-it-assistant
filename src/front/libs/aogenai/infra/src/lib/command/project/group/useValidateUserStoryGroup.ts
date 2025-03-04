import { IUserStoryGroupDto } from '@aogenai/domain';
import {
  MutationHookOptions,
  MutationTuple,
  useMutation,
} from '@apollo/client';
import { GetProjectQuery } from '../cqrs';
import { GetUserStoryGroupQuery, ValidateUserStoryGroupMutation } from './cqrs';

interface Request {
  projectId: string;
  id: string;
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
      projectId: options?.variables?.projectId ?? '',
      id: options?.variables?.id ?? '',
      input: {},
    },
    refetchQueries: [
      GetUserStoryGroupQuery,
      GetUserStoryGroupQuery,
      GetProjectQuery,
    ],
  }) as unknown as MutationTuple<Response, Request>;
