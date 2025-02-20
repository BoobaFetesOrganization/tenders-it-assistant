import { gql } from '@apollo/client';

export const UpdateUserStoryGroupRequestMutation = gql`
  mutation UpdateUserStoryGroupRequest(
    $projectId: String!
    $input: IUserStoryGroupDto!
  ) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.input.id}/request"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
