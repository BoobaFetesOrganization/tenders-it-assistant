import { gql } from '@apollo/client';

export const GetUserStoryGroupQuery = gql`
  query GetUserStoryGroup($projectId: String!, $id: String!) {
    group(projectId: $projectId, id: $id)
      @rest(
        type: "IUserStoryGroupDto"
        method: "GET"
        path: "/project/{args.projectId}/group/{args.id}"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
