import { gql } from '@apollo/client';

export const ValidateUserStoryGroupMutation = gql`
  mutation ValidateUserStoryGroup(
    $projectId: String!
    $input: IUserStoryGroupDto!
  ) {
    group(projectId: $projectId, id: $id, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.id}/validate"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
