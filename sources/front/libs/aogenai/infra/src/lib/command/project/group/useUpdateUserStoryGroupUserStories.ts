import { IUserStoryGroupDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { GetUserStoryGroupQuery } from './cqrs';
import { UpdateUserStoryGroupUserStoriesMutation } from './cqrs/UpdateUserStoryGroupUserStoriesMutation';

interface Request {
  projectId: number;
  input: IUserStoryGroupDto;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useUpdateUserStoryGroupUserStories = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateUserStoryGroupUserStoriesMutation, {
    ...options,
    refetchQueries: [GetUserStoryGroupQuery, GetUserStoryGroupQuery],
  });
