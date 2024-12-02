import { useParams } from 'react-router';
import { IProjectParams } from './IProjectParams';

export function useProjectParams(): IProjectParams {
  const { id } = useParams<{ id: string }>();
  return { id: id ? +id : 0 };
}
