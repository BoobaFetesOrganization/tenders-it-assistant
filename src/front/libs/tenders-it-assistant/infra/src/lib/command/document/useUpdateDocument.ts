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
    {
      ...options,
      onCompleted(data, clientOptions) {
        clientOptions?.client?.refetchQueries({
          include: [GetDocumentQuery, GetDocumentsQuery],
        });
      },
    }
  );
}
