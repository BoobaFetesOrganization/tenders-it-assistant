import { DashBoard } from '@aogenai/application';
import { IProjectBaseDto, IProjectDto } from '@aogenai/domain';
import { useDeleteProject } from '@aogenai/infra';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { projectRouteMapping } from './project';
import { routeMapping } from './project/routeMapping';

export const DashBoardWrapper: FC = memo(() => {
  const navigate = useNavigate();

  const onProjectEstimate = useCallback(
    (project: IProjectDto) => {
      const route = projectRouteMapping.urlToEditor(project);
      navigate(route.to, route);
    },
    [navigate]
  );

  const onProjectSelected = useCallback(
    (id: number) => {
      const route = routeMapping.url({ id });
      navigate(route.to, route);
    },
    [navigate]
  );
  const onProjectCreated = useCallback(
    (item: IProjectBaseDto) => {
      const route = routeMapping.url(item);
      navigate(route.to, route);
    },
    [navigate]
  );

  const [remove] = useDeleteProject();
  const onProjectDeleted = useCallback(
    (item: IProjectBaseDto) => {
      remove({ variables: { id: item.id } });
    },
    [remove]
  );
  return (
    <DashBoard
      onProjectEstimate={onProjectEstimate}
      onProjectSelected={onProjectSelected}
      onProjectCreated={onProjectCreated}
      onProjectDeleted={onProjectDeleted}
    />
  );
});
