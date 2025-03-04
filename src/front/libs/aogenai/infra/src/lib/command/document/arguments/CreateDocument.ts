import { IDocumentDto } from '@aogenai/domain';

export interface CreateDocumentRequest {
  projectId: string;
  file: File;
}

export interface CreateDocumentResponse {
  document: IDocumentDto;
}
