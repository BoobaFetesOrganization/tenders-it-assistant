import { gql } from '@apollo/client';

export const GetDocumentQuery = gql`
  query GetDocument($projectId: String!, $id: String!) {
    document(projectId: $projectId, id: $id)
      @rest(
        type: "IDocumentDto"
        method: "GET"
        path: "/project/{args.projectId}/document/{args.id}"
      ) {
      id
      name
      content
      mimeType
    }
  }
`;
