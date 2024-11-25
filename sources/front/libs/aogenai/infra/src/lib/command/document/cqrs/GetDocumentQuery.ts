import { gql } from '@apollo/client';

export const GetDocumentQuery = gql`
  query GetDocument($id: Int!) {
    documents(id: $id)
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
