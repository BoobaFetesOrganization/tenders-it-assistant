import { QueryHookOptions, useQuery } from '@apollo/client';
import { IPaged, IUserStoryGroupBaseDto } from '@tenders-it-assistant/domain';
import { newPaginationParameter, PaginationParameter } from '../../common';
import { GetUserStoryGroupsQuery } from './cqrs';

interface GetUserStoryGroupsRequest extends PaginationParameter {
  projectId: string;
}
export interface GetUserStoryGroupsResponse {
  groups: IPaged<IUserStoryGroupBaseDto>;
}

export const useUserStoryGroups = (
  options?: QueryHookOptions<
    GetUserStoryGroupsResponse,
    GetUserStoryGroupsRequest
  >
) => {
  const projectId = options?.variables?.projectId ?? '';

  return useQuery<GetUserStoryGroupsResponse, GetUserStoryGroupsRequest>(
    GetUserStoryGroupsQuery,
    {
      ...options,
      skip: !projectId,
      variables: { ...newPaginationParameter(options?.variables), projectId },
    }
  );
};
