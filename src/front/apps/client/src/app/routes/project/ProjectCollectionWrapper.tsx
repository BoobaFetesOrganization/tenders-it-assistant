import { ProjectCollection } from '@tenders-it-assistant/application';
import { FC, memo } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';

export const ProjectCollectionWrapper: FC = memo(() => {
  const navigate = useNavigate();
  return (
    <ProjectCollection
      onCreated={({ id }) => navigate(routeMapping.url({ id }).to)}
      onSelect={({ id }) => navigate(routeMapping.url({ id }).to)}
    />
  );
});
