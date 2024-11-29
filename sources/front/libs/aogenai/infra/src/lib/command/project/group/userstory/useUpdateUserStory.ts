import { IUserStoryDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import {
  GetUserStoriesQuery,
  GetUserStoryQuery,
  UpdateUserStoryMutation,
} from './cqrs';

interface Request {
  projectId: number;
  groupId: number;
  input: IUserStoryDto;
}
interface Response {
  userstory: IUserStoryDto;
}

export const useUpdateUserStory = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateUserStoryMutation, {
    ...options,
    refetchQueries: [GetUserStoriesQuery, GetUserStoryQuery],
  });
