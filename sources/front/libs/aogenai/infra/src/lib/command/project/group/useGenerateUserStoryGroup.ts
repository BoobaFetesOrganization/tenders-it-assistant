import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { GenerateUserStoryGroupMutation, GetUserStoryGroupQuery } from './cqrs';

interface Request {
  projectId: number;
  input: object;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useGenerateUserStoryGroup = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(GenerateUserStoryGroupMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
