import { gql } from '@apollo/client';

export const UpdateUserStoryGroupMutation = gql`
  mutation UpdateUserStoryGroup(
    $projectId: String!
    $input: IUserStoryGroupDto!
  ) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
