import { IDocumentBaseDto, newDocumentBaseDto } from './IDocumentBaseDto';

export interface IDocumentDto extends IDocumentBaseDto {
  content: Uint8Array;
}

export function newDocumentDto(obj?: Partial<IDocumentDto>): IDocumentDto {
  return {
    ...newDocumentBaseDto(obj),
    content: obj?.content || new Uint8Array(),
  };
}
