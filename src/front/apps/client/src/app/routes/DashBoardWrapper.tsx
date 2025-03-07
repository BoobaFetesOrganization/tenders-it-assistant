import { DashBoard } from '@tenders-it-assistant/application';
import { IProjectBaseDto, IProjectDto } from '@tenders-it-assistant/domain';
import { useDeleteProject } from '@tenders-it-assistant/infra';
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
    ({ id }: IProjectDto) => {
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
