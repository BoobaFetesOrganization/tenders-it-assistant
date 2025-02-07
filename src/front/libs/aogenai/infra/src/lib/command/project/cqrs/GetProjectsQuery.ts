import { gql } from '@apollo/client';

export const GetProjectsQuery = gql`
  query GetProjects($limit: Int!, $offset: Int!) {
    projects(limit: $limit, offset: $offset)
      @rest(
        type: "[IProjectBaseDto]"
        method: "GET"
        path: "/project?limit={args.limit}&offset={args.offset}"
      ) {
      page
      data
    }
  }
`;
