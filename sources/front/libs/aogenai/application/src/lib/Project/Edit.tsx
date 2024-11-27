import { IProjectDto, newProjectDto } from '@aogenai/domain';
import { useDeleteProject, useProject, useUpdateProject } from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { ProjectItem } from './Item';

interface IEditProps {
  id: number;
  onSaved?: (item: IProjectDto) => void;
  onDeleted?: (item: IProjectDto) => void;
}
export const ProjectEdit: FC<IEditProps> = memo(
  ({ id, onSaved, onDeleted }) => {
    const [initial, setInitial] = useState(newProjectDto);

    const { loading } = useProject({
      variables: { id },
      onCompleted({ project }) {
        setInitial(project);
      },
    });

    const [call] = useUpdateProject({
      onCompleted({ project }) {
        alert(`Project updated`);
        onSaved?.(project);
      },
    });

    const [deleteProject] = useDeleteProject({
      onCompleted({ project }) {
        onDeleted?.(project);
      },
    });

    const save = useCallback(
      (data: IProjectDto) => {
        call({ variables: { input: data } });
      },
      [call]
    );

    const remove = useCallback(
      (data: IProjectDto) => {
        deleteProject({ variables: { id: data.id } });
      },
      [deleteProject]
    );

    const reset = useCallback(() => initial, [initial]);

    return loading ? (
      'is loading'
    ) : (
      <ProjectItem
        className="edit-project"
        data={initial}
        reset={reset}
        save={save}
        remove={remove}
      />
    );
  }
);
