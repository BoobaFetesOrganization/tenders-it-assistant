import { IPaged, IUserStoryBaseDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { newPaginationParameter, PaginationParameter } from '../../../common';
import { GetUserStoryQuery } from './cqrs';

interface GetUserStoryRequest extends PaginationParameter {
  projectId: number;
  groupId: number;
}
export interface GetUserStoryResponse {
  userstories: IPaged<IUserStoryBaseDto>;
}

export const useUserStories = (
  options?: QueryHookOptions<GetUserStoryResponse, GetUserStoryRequest>
) => {
  const projectId = options?.variables?.projectId ?? 0;
  const groupId = options?.variables?.groupId ?? 0;

  return useQuery<GetUserStoryResponse, GetUserStoryRequest>(
    GetUserStoryQuery,
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
