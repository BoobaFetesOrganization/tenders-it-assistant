import { gql } from '@apollo/client';

export const GetProjectsQuery = gql`
  query GetProjects {
    projects @rest(type: "[IProjectBaseDto]", method: "GET", path: "/project") {
      id
      name
    }
  }
`;
