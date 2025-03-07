import { IDocumentDto } from '@tenders-it-assistant/domain';

export interface CreateDocumentRequest {
  projectId: string;
  file: File;
}

export interface CreateDocumentResponse {
  document: IDocumentDto;
}
