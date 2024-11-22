import { gql } from '@apollo/client';

export const CreateProjectMutation = gql`
  mutation CreateProject {
    project @rest(type: "IProjectDto", method: "POST", path: "/project") {
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
