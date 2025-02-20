import { gql } from '@apollo/client';

export const DeleteUserStoryMutation = gql`
  mutation DeleteUserStory(
    $projectId: String!
    $groupId: String!
    $id: String!
  ) {
    userstory(projectId: $projectId, groupId: $groupId, id: $id)
      @rest(
        type: "IUserStoryDto"
        method: "DELETE"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory/{args.id}"
      ) {
      id
      groupId
      name
      cost
      tasks
    }
  }
`;
