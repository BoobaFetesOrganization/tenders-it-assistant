import { gql } from '@apollo/client';

export const DeleteUserStoryMutation = gql`
  mutation DeleteUserStory($projectId: Int!, $groupId: Int!, $id: Int!) {
    userstory(projectId: $projectId, groupId: $groupId, id: $id)
      @rest(
        type: "IUserStoryDto"
        method: "DELETE"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory/{args.id}"
      ) {
      id
      name
      cost
      tasks
    }
  }
`;
