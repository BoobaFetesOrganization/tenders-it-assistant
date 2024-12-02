import { ProjectCollection } from '@aogenai/application';
import { FC, memo } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';

export const ProjectCollectionWrapper: FC = memo(() => {
  const navigate = useNavigate();
  return (
    <ProjectCollection
      onCreate={() => navigate(routeMapping.url({ id: 0 }).to)}
      onSelect={({ id }) => navigate(routeMapping.url({ id }).to)}
    />
  );
});
