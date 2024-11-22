import { gql } from '@apollo/client';

export const GetDocumentsQuery = gql`
  query GetDocument {
    documents
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
