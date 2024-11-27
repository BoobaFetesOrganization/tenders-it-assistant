import { IDocumentDto } from '@aogenai/domain';

export interface CreateDocumentRequest {
  projectId: number;
  file: File;
}

export interface CreateDocumentResponse {
  document: IDocumentDto;
}
