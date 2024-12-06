import { gql } from '@apollo/client';

export const CreateProjectMutation = gql`
  mutation CreateProject($input: IProjectBaseDto!) {
    project(input: $input)
      @rest(type: "IProjectDto", method: "POST", path: "/project") {
      id
      name
      documents
      selectedGroup
    }
  }
`;
