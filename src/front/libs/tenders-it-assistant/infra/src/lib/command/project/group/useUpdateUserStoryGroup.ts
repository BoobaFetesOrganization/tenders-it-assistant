import { MutationHookOptions, useMutation } from '@apollo/client';
import { IUserStoryGroupDto } from '@tenders-it-assistant/domain';
import { GetUserStoryGroupQuery, UpdateUserStoryGroupMutation } from './cqrs';

interface Request {
  projectId: string;
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
