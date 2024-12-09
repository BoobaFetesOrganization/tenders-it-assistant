import { DashBoard } from '@aogenai/application';
import { IProjectBaseDto } from '@aogenai/domain';
import { useDeleteProject } from '@aogenai/infra';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './project/routeMapping';

export const DashBoardWrapper: FC = memo(() => {
  const navigate = useNavigate();
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
      onProjectSelected={onProjectSelected}
      onProjectCreated={onProjectCreated}
      onProjectDeleted={onProjectDeleted}
    />
  );
});
