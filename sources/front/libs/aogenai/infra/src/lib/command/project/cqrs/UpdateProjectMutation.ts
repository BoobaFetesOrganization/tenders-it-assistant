import { gql } from '@apollo/client';

export const UpdateProjectMutation = gql`
  mutation UpdateProject($input: IProjectDto!) {
    project(input: $input)
      @rest(type: "IProjectDto", method: "PUT", path: "/project") {
      id
      name
      prompt
      responseId
      documents {
        id
        name
      }
      userStories {
        id
        name
        cost
      }
    }
  }
`;
