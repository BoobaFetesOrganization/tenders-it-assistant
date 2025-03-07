import { gql } from '@apollo/client';

export const DeleteUserStoryGroupMutation = gql`
  mutation DeleteUserStoryGroup($projectId: String!, $id: String!) {
    group(projectId: $projectId, id: $id)
      @rest(
        type: "IUserStoryGroupDto"
        method: "DELETE"
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
