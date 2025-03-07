import { gql } from '@apollo/client';

export const GetProjectQuery = gql`
  query GetProject($id: String!) {
    project(id: $id)
      @rest(type: "IProjectDto", method: "GET", path: "/project/{args.id}") {
      id
      name
      documents
      selectedGroup
    }
  }
`;
