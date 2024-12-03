import { gql } from '@apollo/client';

export const GetUserStoryGroupQuery = gql`
  query GetUserStoryGroup($projectId: Int!, $id: Int!) {
    group(projectId: $projectId, id: $id)
      @rest(
        type: "IUserStoryGroupDto"
        method: "GET"
        path: "/project/{args.projectId}/group/{args.id}"
      ) {
      id
      request
      response
      userStories
    }
  }
`;
