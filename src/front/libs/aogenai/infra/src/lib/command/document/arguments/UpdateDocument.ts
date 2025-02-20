import { IDocumentDto } from '@aogenai/domain';

export interface UpdateDocumentRequest {
  projectId: string;
  id: string;
  file: File;
}

export interface UpdateDocumentResponse {
  document: IDocumentDto;
}
