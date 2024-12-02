import { UserStoryGroupCreate, UserStoryGroupEdit } from '@aogenai/application';
import { IUserStoryGroupDto } from '@aogenai/domain';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';
import { useUserstoryGroupParams } from './useUserstoryGroupParams';

export const UserStoryGroupItemWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const { projectId, id } = useUserstoryGroupParams();

  const navigateToEdit = useCallback(
    ({ id }: IUserStoryGroupDto) =>
      navigate(routeMapping.url({ projectId, id }).to),
    [navigate, projectId]
  );
  const navigateToList = useCallback(
    () => navigate(routeMapping.url({ projectId }).to),
    [navigate, projectId]
  );

  return id === 0 ? (
    <UserStoryGroupCreate projectId={projectId} onCreated={navigateToEdit} />
  ) : (
    <UserStoryGroupEdit
      projectId={projectId}
      id={id}
      onDeleted={navigateToList}
    />
  );
});
