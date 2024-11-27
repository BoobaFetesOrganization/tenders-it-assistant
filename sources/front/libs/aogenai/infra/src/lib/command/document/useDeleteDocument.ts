import { IDocumentDto } from '@aogenai/domain';
import { MutationHookOptions, useMutation } from '@apollo/client';
import { DeleteDocumentMutation, GetDocumentsQuery } from './cqrs';

interface Request {
  projectId: number;
  id: number;
}
interface Response {
  document: IDocumentDto;
}

export const useDeleteDocument = (
  options?: MutationHookOptions<Response, Request>
) =>
  useMutation<Response, Request>(DeleteDocumentMutation, {
    ...options,
    refetchQueries: [GetDocumentsQuery],
  });
