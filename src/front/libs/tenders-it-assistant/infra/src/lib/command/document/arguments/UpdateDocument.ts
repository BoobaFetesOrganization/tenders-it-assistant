import { IDocumentDto } from '@tenders-it-assistant/domain';

export interface UpdateDocumentRequest {
  projectId: string;
  id: string;
  file: File;
}

export interface UpdateDocumentResponse {
  document: IDocumentDto;
}
