import { IProjectDto, newProjectDto } from '@aogenai/domain';
import { useProject, useUpdateProject } from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { ProjectItem } from './Item';

interface IEditProps {
  id: number;
}
export const ProjectEdit: FC<IEditProps> = memo(({ id }) => {
  const [intial, setInitial] = useState(newProjectDto);

  const { data: { project } = { project: newProjectDto() }, loading } =
    useProject({
      variables: { id },
      onCompleted({ project }) {
        setInitial(project);
      },
    });

  const [call] = useUpdateProject({
    onCompleted() {
      alert(`Project updated`);
    },
  });

  const save = useCallback(
    (data: IProjectDto) => {
      call({ variables: { input: data } });
    },
    [call]
  );

  const reset = useCallback(() => intial, [intial]);

  return loading ? (
    'is loading'
  ) : (
    <ProjectItem data={project} reset={reset} save={save} />
  );
});
