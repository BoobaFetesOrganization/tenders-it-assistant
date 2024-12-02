import { useParams } from 'react-router';
import { IUserStoryGroupParams } from './IUserStoryGroupParams';

export function useUserstoryGroupParams(): IUserStoryGroupParams {
  const { projectId: projectIdParam, id } = useParams<{
    projectId: string;
    id: string;
  }>();

  const projectId = projectIdParam ? +projectIdParam : NaN;

  if (isNaN(projectId))
    throw new Error('Project id is missing or not a number');

  return { projectId, id: id ? +id : 0 };
}
