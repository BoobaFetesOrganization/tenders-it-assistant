import { gql } from '@apollo/client';

export const GenerateUserStoryGroupMutation = gql`
  mutation GenerateUserStoryGroup($projectId: Int!, $input: Object!) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group/generate"
      ) {
      id
      request
      response
      userStories
    }
  }
`;
