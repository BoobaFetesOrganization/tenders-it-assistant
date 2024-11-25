import { gql } from '@apollo/client';

export const GetProjectQuery = gql`
  query GetProject($id: Int!) {
    projects(id: $id)
      @rest(type: "IProjectDto", method: "GET", path: "/project/{args.id}") {
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
