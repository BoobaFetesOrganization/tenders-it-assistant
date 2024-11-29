import { gql } from '@apollo/client';

export const ValidateUserStoryGroupMutation = gql`
  mutation ValidateUserStoryGroup($projectId: Int!, $input: Object!) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/validate"
      ) {
      id
      request
      response
      userStories
    }
  }
`;
