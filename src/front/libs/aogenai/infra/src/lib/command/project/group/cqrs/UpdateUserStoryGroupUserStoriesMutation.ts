import { gql } from '@apollo/client';

export const UpdateUserStoryGroupUserStoriesMutation = gql`
  mutation UpdateUserStoryGroupUserStories(
    $projectId: String!
    $input: IUserStoryGroupDto!
  ) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/{args.input.id}/story"
      ) {
      id
      projectId
      request
      response
      userStories
    }
  }
`;
