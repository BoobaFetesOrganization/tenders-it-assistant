import { gql } from '@apollo/client';

export const CreateUserStoryMutation = gql`
  mutation CreateUserStory(
    $projectId: String!
    $groupId: String!
    $input: IUserStoryDto!
  ) {
    userstory(projectId: $projectId, groupId: $groupId, input: $input)
      @rest(
        type: "IUserStoryDto"
        method: "POST"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory"
      ) {
      id
      groupId
      name
      cost
      tasks
    }
  }
`;
