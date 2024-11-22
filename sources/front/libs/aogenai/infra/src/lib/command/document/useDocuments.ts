import { IDocumentBaseDto } from '@aogenai/domain';
import { gql, QueryHookOptions, useQuery } from '@apollo/client';

interface Request {
  projectId: number;
}
interface Response {
  documents: IDocumentBaseDto[];
}

export const GetDocumentsQuery = gql`
  query GetDocuments {
    documents
      @rest(
        type: "[IDocumentBaseDto]"
        method: "GET"
        path: "/project/{args.projectId}/document"
      ) {
      id
      name
    }
  }
`;

export const useDocuments = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetDocumentsQuery, options);
