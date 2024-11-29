import { gql } from '@apollo/client';

export const GetProjectQuery = gql`
  query GetProject($id: Int!) {
    project(id: $id)
      @rest(type: "IProjectDto", method: "GET", path: "/project/{args.id}") {
      id
      name
      documents
      stories
    }
  }
`;
