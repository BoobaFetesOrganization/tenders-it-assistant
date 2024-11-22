import { gql } from '@apollo/client';

export const UpdateProjectMutation = gql`
  mutation UpdateProject {
    project @rest(type: "IProjectDto", method: "PUT", path: "/project") {
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
