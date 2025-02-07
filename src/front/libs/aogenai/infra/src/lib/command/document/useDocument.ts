import { IDocumentDto } from '@aogenai/domain';
import { QueryHookOptions, useQuery } from '@apollo/client';
import { GetDocumentQuery } from './cqrs';

interface Request {
  projectId: number;
  id: number;
}
interface Response {
  document: IDocumentDto;
}

export const useDocument = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetDocumentQuery, options);
