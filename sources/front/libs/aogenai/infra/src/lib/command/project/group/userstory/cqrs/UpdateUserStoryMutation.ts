import { gql } from '@apollo/client';

export const UpdateUserStoryMutation = gql`
  mutation UpdateUserStory(
    $projectId: Int!
    $groupId: Int!
    $input: IUserStoryDto!
  ) {
    userstory(projectId: $projectId, groupId: $groupId, input: $input)
      @rest(
        type: "IUserStoryDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory"
      ) {
      id
      name
      cost
      tasks
    }
  }
`;
