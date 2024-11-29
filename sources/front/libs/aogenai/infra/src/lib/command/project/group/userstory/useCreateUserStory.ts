import { IUserStoryDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import {
  CreateUserStoryMutation,
  GetUserStoriesQuery,
  GetUserStoryQuery,
} from './cqrs';

interface Request {
  projectId: number;
  groupId: number;
  input: IUserStoryDto;
}
interface Response {
  userstory: IUserStoryDto;
}

export const useCreateUserStory = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(CreateUserStoryMutation, {
    ...options,
    refetchQueries: [GetUserStoriesQuery, GetUserStoryQuery],
  });
