import { gql } from '@apollo/client';

export const UpdateUserStoryGroupMutation = gql`
  mutation UpdateUserStoryGroup($projectId: Int!, $input: IUserStoryGroupDto!) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "PUT"
        path: "/project/{args.projectId}/group"
      ) {
      id
      request
      response
      userStories
    }
  }
`;
