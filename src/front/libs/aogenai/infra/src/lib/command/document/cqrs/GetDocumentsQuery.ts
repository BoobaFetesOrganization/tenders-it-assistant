import { gql } from '@apollo/client';

export const GetDocumentsQuery = gql`
  query GetDocuments($projectId: String!, $limit: Int!, $offset: Int!) {
    documents(projectId: $projectId, limit: $limit, offset: $offset)
      @rest(
        type: "[IDocumentBaseDto]"
        method: "GET"
        path: "/project/{args.projectId}/document?limit={args.limit}&offset={args.offset}"
      ) {
      page
      data
    }
  }
`;
