import { gql } from '@apollo/client';

export const GetUserStoriesQuery = gql`
  query GetUserStorys(
    $projectId: String!
    $groupId: String!
    $limit: Int!
    $offset: Int!
  ) {
    stories(
      projectId: $projectId
      groupId: $groupId
      limit: $limit
      offset: $offset
    )
      @rest(
        type: "[IUserStoryBaseDto]"
        method: "GET"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory?limit={args.limit}&offset={args.offset}"
      ) {
      page
      data
    }
  }
`;
