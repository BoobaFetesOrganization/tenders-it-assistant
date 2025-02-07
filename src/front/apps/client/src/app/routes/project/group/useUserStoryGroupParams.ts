import { useParams } from 'react-router';
import { IUserStoryGroupParams } from './IUserStoryGroupParams';

export function useUserStoryGroupParams(): IUserStoryGroupParams {
  const { projectId, groupId } = useParams<{
    projectId: string;
    groupId: string;
  }>();
  return { projectId: projectId ? +projectId : 0, id: groupId ? +groupId : 0 };
}
