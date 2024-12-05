import { IDocumentBaseDto, newDocumentBaseDto } from './IDocumentBaseDto';

export interface IDocumentDto extends IDocumentBaseDto {
  mimeType: string;
  content: string;
}

export function newDocumentDto(obj?: Partial<IDocumentDto>): IDocumentDto {
  return {
    ...newDocumentBaseDto(obj),
    mimeType: obj?.mimeType || '',
    content: obj?.content || '',
  };
}
