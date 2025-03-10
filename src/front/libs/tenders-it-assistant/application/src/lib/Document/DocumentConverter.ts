import { IDocumentDto } from '@tenders-it-assistant/domain';

export class DocumentConverter {
  toBlob({ content, mimeType }: IDocumentDto): Blob {
    const decodedContent = atob(content);
    const bytes = new Uint8Array(decodedContent.length);
    for (let i = 0; i < decodedContent.length; i++) {
      bytes[i] = decodedContent.charCodeAt(i);
    }
    return new Blob([bytes], { type: mimeType });
  }
}
