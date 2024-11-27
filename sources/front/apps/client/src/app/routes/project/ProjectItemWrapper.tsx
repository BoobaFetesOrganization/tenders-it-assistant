import { ProjectCreate, ProjectEdit } from '@aogenai/application';
import { IProjectDto } from '@aogenai/domain';
import { FC, memo, useCallback } from 'react';
import { useNavigate, useParams } from 'react-router';

export const ProjectItemWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const params = useParams<{ id: string }>();
  const id = params?.id ? +params.id : NaN;

  const navigateToEdit = useCallback(
    (item: IProjectDto) => navigate(`/project/${item.id}`),
    [navigate]
  );
  const navigateToList = useCallback(() => navigate(`/project`), [navigate]);

  return id === 0 ? (
    <ProjectCreate onCreated={navigateToEdit} />
  ) : (
    <ProjectEdit id={id} onDeleted={navigateToList} />
  );
});
