import { ProjectStoriesEditor } from '@tenders-it-assistant/application';
import { FC, memo } from 'react';
import { useProjectParams } from './useProjectParams';

export const ProjectStoriesEditorWrapper: FC = memo(() => {
  const { id } = useProjectParams();
  return <ProjectStoriesEditor projectId={id} />;
});
