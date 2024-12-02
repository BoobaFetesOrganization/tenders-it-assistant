import { ProjectCreate, ProjectEdit } from '@aogenai/application';
import { IProjectDto } from '@aogenai/domain';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';
import { useProjectParams } from './useProjectParams';

export const ProjectItemWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const { id } = useProjectParams();

  const navigateToEdit = useCallback(
    (item: IProjectDto) => navigate(routeMapping.url({ id: item.id }).to),
    [navigate]
  );
  const navigateToList = useCallback(
    () => navigate(routeMapping.url().to),
    [navigate]
  );

  return id === 0 ? (
    <ProjectCreate onCreated={navigateToEdit} />
  ) : (
    <ProjectEdit id={id} onDeleted={navigateToList} />
  );
});
