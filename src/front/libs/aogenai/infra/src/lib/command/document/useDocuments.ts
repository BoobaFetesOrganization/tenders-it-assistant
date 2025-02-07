import { IDocumentBaseDto, IPaged } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { getInfraSettings } from '../../settings';
import { GetDocumentsQuery } from './cqrs';

export interface UseDocumentRequest {
  projectId: number;
  limit: number;
  offset: number;
}
interface Response {
  documents: IPaged<IDocumentBaseDto>;
}

export const useDocuments = (
  options?: QueryHookOptions<Response, UseDocumentRequest>
) => {
  const maxLimit = getInfraSettings().api.maxLimit;

  return useQuery<Response, UseDocumentRequest>(GetDocumentsQuery, {
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
