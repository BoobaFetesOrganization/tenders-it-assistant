import { gql } from '@apollo/client';

export const GetDocumentsQuery = gql`
  query GetDocument($limit: Int!, $offset: Int!) {
    documents(limit: $limit, offset: $offset)
      @rest(
        type: "IDocumentDto"
        method: "GET"
        path: "/project/{args.projectId}/document/{args.id}"
      ) {
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
