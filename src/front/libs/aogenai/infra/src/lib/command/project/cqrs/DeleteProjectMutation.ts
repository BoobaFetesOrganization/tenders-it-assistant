import { gql } from '@apollo/client';

export const DeleteProjectMutation = gql`
  mutation DeleteProject($id: String!) {
    project(id: $id)
      @rest(type: "IProjectDto", method: "DELETE", path: "/project/{args.id}") {
      id
      name
      documents
      selectedGroup
    }
  }
`;
