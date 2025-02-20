import { IPaged, IUserStoryBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { newPaginationParameter, PaginationParameter } from '../../../common';
import { GetUserStoriesQuery } from './cqrs';

interface GetUserStoryRequest extends PaginationParameter {
  projectId: string;
  groupId: string;
}
export interface GetUserStoryResponse {
  stories: IPaged<IUserStoryBaseDto>;
}

export const useUserStories = (
  options?: QueryHookOptions<GetUserStoryResponse, GetUserStoryRequest>
) => {
  const projectId = options?.variables?.projectId ?? '';
  const groupId = options?.variables?.groupId ?? '';

  return useQuery<GetUserStoryResponse, GetUserStoryRequest>(
    GetUserStoriesQuery,
    {
      ...options,
      skip: !projectId || !groupId,
      variables: {
        ...newPaginationParameter(options?.variables),
        projectId,
        groupId,
      },
    }
  );
};
