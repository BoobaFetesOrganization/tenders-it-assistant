import { gql } from '@apollo/client';

export const GenerateUserStoryGroupMutation = gql`
  mutation GenerateUserStoryGroup(
    $projectId: String!
    $id: String!
    $input: IUserStoryGroupDto!
  ) {
    group(projectId: $projectId, id: $id, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.id}/generate"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
