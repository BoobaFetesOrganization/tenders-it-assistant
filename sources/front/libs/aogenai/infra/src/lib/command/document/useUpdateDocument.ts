import { MutationHookOptions, MutationTuple } from '@apollo/client';
import { UpdateDocumentRequest, UpdateDocumentResponse } from './arguments';
import {
  GetDocumentQuery,
  GetDocumentsQuery,
  updateDocumentCommand,
} from './cqrs';
import { useUploadFile } from './cqrs/tools/useUploadFile';

export function useUpdateDocument(
  options?: MutationHookOptions<UpdateDocumentResponse, UpdateDocumentRequest>
): MutationTuple<UpdateDocumentResponse, UpdateDocumentRequest> {
  return useUploadFile<UpdateDocumentResponse, UpdateDocumentRequest>(
    async (variables) =>
      await updateDocumentCommand(
        variables.projectId,
        variables.id,
        variables.file
      ),
    options,
    (client, data) => {
      client.refetchQueries({
        include: [GetDocumentsQuery, GetDocumentQuery],
      });
    }
  );
}
