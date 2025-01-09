import { IDocumentBaseDto } from '../document';
import { IUserStoryGroupDto } from './group';
import { IProjectBaseDto, newProjectBaseDto } from './IProjectBaseDto';

export interface IProjectDto extends IProjectBaseDto {
  documents: IDocumentBaseDto[];
  selectedGroup?: IUserStoryGroupDto;
}

export function newProjectDto(obj?: Partial<IProjectDto>): IProjectDto {
  return {
    ...newProjectBaseDto(obj),
    documents: obj?.documents || [],
    selectedGroup: obj?.selectedGroup,
  };
}
