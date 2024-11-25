import { FC, memo } from 'react';
import { useParams } from 'react-router';

export const ProjectEdit: FC = memo(() => {
  const params = useParams<{ id: string }>();
  const id = params?.id ? +params.id : NaN;

  return id === 0 ? 'Create project' : 'edit project #' + id;
});
