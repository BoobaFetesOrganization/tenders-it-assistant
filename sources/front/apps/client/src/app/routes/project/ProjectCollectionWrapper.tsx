import { ProjectCollection } from '@aogenai/application';
import { FC, memo } from 'react';
import { useNavigate } from 'react-router';

export const ProjectCollectionWrapper: FC = memo(() => {
  const navigate = useNavigate();
  return (
    <ProjectCollection
      onCreate={() => navigate({ pathname: '/project/0' })}
      onSelect={({ id }) => navigate({ pathname: `/project/${id}` })}
    />
  );
});
