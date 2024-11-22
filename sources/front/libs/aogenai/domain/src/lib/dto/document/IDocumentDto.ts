import { IDocumentBaseDto, newDocumentBaseDto } from './IDocumentBaseDto';

export interface IDocumentDto extends IDocumentBaseDto {
  content: Uint8Array;
  createTime: Date;
  updateTime: Date;
}

export function newDocumentDto(obj?: Partial<IDocumentDto>): IDocumentDto {
  const now = new Date(Date.now());
  return {
    ...newDocumentBaseDto(obj),
    content: obj?.content || new Uint8Array(),
    createTime: obj?.createTime || now,
    updateTime: obj?.updateTime || now,
  };
}
