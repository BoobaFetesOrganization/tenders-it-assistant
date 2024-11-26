import { ProjectCreate, ProjectEdit } from '@aogenai/application';
import { FC, memo } from 'react';
import { useParams } from 'react-router';

export const ProjectItemWrapper: FC = memo(() => {
  const params = useParams<{ id: string }>();
  const id = params?.id ? +params.id : NaN;

  return id === 0 ? <ProjectCreate /> : <ProjectEdit id={id} />;
});
