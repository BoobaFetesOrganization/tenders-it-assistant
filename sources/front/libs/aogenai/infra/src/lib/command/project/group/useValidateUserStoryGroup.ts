import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { GetUserStoryGroupQuery, ValidateUserStoryGroupMutation } from './cqrs';

interface Request {
  projectId: number;
  input: object;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useValidateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(ValidateUserStoryGroupMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
