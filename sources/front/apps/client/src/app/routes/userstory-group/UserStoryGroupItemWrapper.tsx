import { DataNotFound, UserStoryGroupEdit } from '@aogenai/application';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';
import { useUserstoryGroupParams } from './useUserstoryGroupParams';

export const UserStoryGroupItemWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const { projectId, id } = useUserstoryGroupParams();

  const navigateToList = useCallback(
    () => navigate(routeMapping.url({ projectId }).to),
    [navigate, projectId]
  );

  return id === 0 ? (
    <DataNotFound />
  ) : (
    <UserStoryGroupEdit
      projectId={projectId}
      id={id}
      onDeleted={navigateToList}
    />
  );
});
