import { UserStoryGroupCollection } from '@aogenai/application';
import { FC, memo } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';
import { useUserstoryGroupParams } from './useUserstoryGroupParams';

export const UserStoryGroupCollectionWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const params = useUserstoryGroupParams();

  return (
    <UserStoryGroupCollection
      projectId={params.projectId}
      onCreate={() => navigate(routeMapping.url(params).to)}
      onSelect={({ id }) => navigate(routeMapping.url({ ...params, id }).to)}
    />
  );
});
