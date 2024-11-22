import { IDocumentDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import {
  CreateDocumentMutation,
  GetDocumentQuery,
  GetDocumentsQuery,
} from './cqrs';

interface Request {
  projectId: number;
  input: { name: string };
}
interface Response {
  document: IDocumentDto;
}

export const useCreateDocument = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(CreateDocumentMutation, {
    ...options,
    refetchQueries: [GetDocumentQuery, GetDocumentsQuery],
  });
