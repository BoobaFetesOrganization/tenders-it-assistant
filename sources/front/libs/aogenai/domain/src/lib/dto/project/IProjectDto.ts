import { IDocumentBaseDto } from '../document';
import { IUserStoryBaseDto } from '../userstory/IUserStoryBaseDto';
import { IProjectBaseDto, newProjectBaseDto } from './IProjectBaseDto';

export interface IProjectDto extends IProjectBaseDto {
  prompt: string;
  responseId: number;
  documents: IDocumentBaseDto[];
  userStories: IUserStoryBaseDto[];
}

export function newProjectDto(obj?: Partial<IProjectDto>): IProjectDto {
  return {
    ...newProjectBaseDto(obj),
    prompt: obj?.prompt || '',
    responseId: obj?.responseId || 0,
    documents: obj?.documents || [],
    userStories: obj?.userStories || [],
  };
}
