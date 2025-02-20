import { IUserStoryDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import {
  CreateUserStoryMutation,
  GetUserStoriesQuery,
  GetUserStoryQuery,
} from './cqrs';

interface Request {
  projectId: string;
  groupId: string;
  input: IUserStoryDto;
}
interface Response {
  userstory: IUserStoryDto;
}

export const useCreateUserStory = (
  options?: MutationHookOptions<Response, Request>
) => {
  return useMutation<Response, Request>(CreateUserStoryMutation, {
    ...options,
    refetchQueries: [GetUserStoriesQuery, GetUserStoryQuery],
  });
};
