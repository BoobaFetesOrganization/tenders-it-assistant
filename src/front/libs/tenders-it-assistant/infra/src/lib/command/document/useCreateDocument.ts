import { MutationHookOptions, MutationTuple } from '@apollo/client';
import { CreateDocumentRequest, CreateDocumentResponse } from './arguments';
import { createDocumentCommand, GetDocumentsQuery } from './cqrs';
import { useUploadFile } from './cqrs/tools/useUploadFile';

export function useCreateDocument(
  options?: MutationHookOptions<CreateDocumentResponse, CreateDocumentRequest>
): MutationTuple<CreateDocumentResponse, CreateDocumentRequest> {
  return useUploadFile<CreateDocumentResponse, CreateDocumentRequest>(
    async (variables) =>
      await createDocumentCommand(variables.projectId, variables.file),
    {
      ...options,
      onCompleted(data, opt) {
        opt?.client?.refetchQueries({
          include: [GetDocumentsQuery],
        });
      },
    }
  );
}
