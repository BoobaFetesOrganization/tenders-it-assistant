import { gql } from '@apollo/client';

export const CreateUserStoryGroupMutation = gql`
  mutation CreateUserStoryGroup($projectId: Int!, $input: IUserStoryGroupDto!) {
    group(projectId: $projectId, input: $input)
      @rest(
        type: "IUserStoryGroupDto"
        method: "POST"
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
