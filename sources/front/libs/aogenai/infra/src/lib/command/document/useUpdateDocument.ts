import { IDocumentDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import {
  GetDocumentQuery,
  GetDocumentsQuery,
  UpdateDocumentMutation,
} from './cqrs';

interface Request {
  input: { name: string };
}
interface Response {
  document: IDocumentDto;
}

export const useUpdateDocument = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(UpdateDocumentMutation, {
    ...options,
    refetchQueries: [GetDocumentQuery, GetDocumentsQuery],
  });
