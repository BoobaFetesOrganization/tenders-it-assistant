import { IProjectDto, newProjectDto } from '@aogenai/domain';
import { useCreateProject } from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { ProjectItem } from './Item';

interface IProjectCreateProps {
  onCreated: (item: IProjectDto) => void;
}
export const ProjectCreate: FC<IProjectCreateProps> = memo(({ onCreated }) => {
  const [intial, setInitial] = useState(newProjectDto);

  const [call] = useCreateProject({
    onCompleted({ project }) {
      if (!project.id) onCreated(project);
      else setInitial(project);
    },
  });

  const save = useCallback(
    (data: IProjectDto) => {
      call({ variables: { input: data } });
    },
    [call]
  );

  return (
    <ProjectItem
      className="create-project"
      data={intial}
      reset={newProjectDto}
      save={save}
    />
  );
});
