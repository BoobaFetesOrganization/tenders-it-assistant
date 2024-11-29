import { IUserStoryDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { GetUserStoryQuery } from './cqrs';

interface Request {
  projectId: number;
  groupId: number;
  id: number;
}
interface Response {
  userstory: IUserStoryDto;
}

export const useUserStory = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetUserStoryQuery, options);
