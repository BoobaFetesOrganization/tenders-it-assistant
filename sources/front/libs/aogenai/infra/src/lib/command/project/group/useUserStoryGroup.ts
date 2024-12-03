import { IUserStoryGroupDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { GetUserStoryGroupQuery } from './cqrs';

interface Request {
  projectId: number;
  id: number;
}
interface Response {
  group: IUserStoryGroupDto;
}

export const useUserStoryGroup = (
  options?: QueryHookOptions<Response, Request>
) =>
  useQuery<Response, Request>(GetUserStoryGroupQuery, {
    ...options,
    skip:
      !options?.variables?.projectId ||
      !options?.variables?.id ||
      options?.skip,
  });
