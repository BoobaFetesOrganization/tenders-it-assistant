import { gql } from '@apollo/client';

export const DeleteUserStoryGroupMutation = gql`
  mutation DeleteUserStoryGroup($projectId: Int!, $id: Int!) {
    group(projectId: $projectId, id: $id)
      @rest(
        type: "IUserStoryGroupDto"
        method: "DELETE"
        path: "/project/{args.projectId}/group/{args.id}"
      ) {
      id
      request
      response
      userStories
    }
  }
`;
