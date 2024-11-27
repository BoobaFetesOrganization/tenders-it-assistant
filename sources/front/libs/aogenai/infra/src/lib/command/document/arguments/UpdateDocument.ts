import { IDocumentDto } from '@aogenai/domain';

export interface UpdateDocumentRequest {
  projectId: number;
  id: number;
  file: File;
}

export interface UpdateDocumentResponse {
  document: IDocumentDto;
}
