import { IDocumentBaseDto, IPaged } from '@aogenai/domain';
import { gql, QueryHookOptions, useQuery } from '@apollo/client';
import { getInfraSettings } from '../../settings';

interface Request {
  projectId: number;
  limit?: number;
  offset?: number;
}
interface Response {
  documents: IPaged<IDocumentBaseDto>;
}

export const GetDocumentsQuery = gql`
  query GetDocuments {
    documents
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

export const useDocuments = (options?: QueryHookOptions<Response, Request>) => {
  const maxLimit = getInfraSettings().api.maxLimit;

  return useQuery<Response, Request>(GetDocumentsQuery, {
    ...options,
    skip: !options?.variables?.projectId || options.variables.projectId < 0,
    variables: {
      offset: 0,
      limit: maxLimit,
      projectId: 0,
      ...options?.variables,
    },
  });
};
