import { useParams } from 'react-router';
import { IProjectParams } from './IProjectParams';

export function useProjectParams(): IProjectParams {
  const { projectId } = useParams<{ projectId: string }>();
  return { id: projectId ? +projectId : 0 };
}
