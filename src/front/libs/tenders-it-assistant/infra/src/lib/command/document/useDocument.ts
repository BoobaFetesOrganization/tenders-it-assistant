import { QueryHookOptions, useQuery } from '@apollo/client';
import { IDocumentDto } from '@tenders-it-assistant/domain';
import { GetDocumentQuery } from './cqrs';

interface Request {
  projectId: string;
  id: string;
}
interface Response {
  document: IDocumentDto;
}

export const useDocument = (options?: QueryHookOptions<Response, Request>) =>
  useQuery<Response, Request>(GetDocumentQuery, options);
