import { gql } from '@apollo/client';

export const GetUserStoryGroupsQuery = gql`
  query GetUserStoryGroups($projectId: Int!, $limit: Int!, $offset: Int!) {
    groups(projectId: $projectId, limit: $limit, offset: $offset)
      @rest(
        type: "[IUserStoryGroupBaseDto]"
        method: "GET"
        path: "/project/{args.projectId}/group?limit={args.limit}&offset={args.offset}"
      ) {
      page
      data
    }
  }
`;
