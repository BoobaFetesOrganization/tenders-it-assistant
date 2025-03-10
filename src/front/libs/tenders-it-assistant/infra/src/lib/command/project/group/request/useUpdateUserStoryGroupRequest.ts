import { MutationHookOptions, useMutation } from '@apollo/client';
import { IUserStoryRequestDto } from '@tenders-it-assistant/domain';
import { GetUserStoryGroupQuery } from '../cqrs';
import { UpdateUserStoryGroupRequestMutation } from './cqrs';

interface Request {
  projectId: string;
  groupId: string;
  input: IUserStoryRequestDto;
}
interface Response {
  request: boolean | null;
}

export const useUpdateUserStoryGroupRequest = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateUserStoryGroupRequestMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery],
  });
