import { gql } from '@apollo/client';

export const UpdateUserStoryGroupRequestMutation = gql`
  mutation UpdateUserStoryGroupRequest(
    $projectId: String!
    $groupId: String!
    $input: IUserStoryRequestDto!
  ) {
    request(projectId: $projectId, groupId: $groupId, input: $input)
      @rest(
        type: "IUserStoryRequestDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.groupId}/request"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
