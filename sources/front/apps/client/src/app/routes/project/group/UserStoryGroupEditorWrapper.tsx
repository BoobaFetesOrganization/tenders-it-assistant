import { UserStoryGroupEditor } from '@aogenai/application';
import { FC, memo } from 'react';
import { useUserStoryGroupParams } from './useUserStoryGroupParams';

export const UserStoryGroupEditorWrapper: FC = memo(() => {
  const { projectId } = useUserStoryGroupParams();
  return <UserStoryGroupEditor projectId={projectId} />;
});
