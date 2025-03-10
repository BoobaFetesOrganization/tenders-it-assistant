import {
  IDocumentDto,
  IProjectDto,
  newProjectDto,
} from '@tenders-it-assistant/domain';
import {
  useDeleteProject,
  useProject,
  useUpdateProject,
} from '@tenders-it-assistant/infra';
import { FC, memo, useCallback, useState } from 'react';
import { IProjectItemProps, ProjectItem } from './Item';

interface IEditProps extends Pick<IProjectItemProps, 'onUserStoryEditorCLick'> {
  id: string;
  onSaved?: (item: IProjectDto) => void;
  onDeleted?: (item: IProjectDto) => void;
  onDocumentDonwloaded?: (document: IDocumentDto) => void;
}
export const ProjectEdit: FC<IEditProps> = memo(
  ({
    id,
    onSaved,
    onDeleted,
    onDocumentDonwloaded,
    onUserStoryEditorCLick,
  }) => {
    const [initial, setInitial] = useState(newProjectDto());
    const [item, setItem] = useState(initial);

    const { loading } = useProject({
      variables: { id },
      onCompleted({ project }) {
        setInitial(project);
        setItem(project);
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

    const onSave = useCallback(() => {
      call({ variables: { input: item } });
    }, [call, item]);

    const onRemove = useCallback(() => {
      deleteProject({ variables: { id: item.id } });
    }, [deleteProject, item.id]);

    const onReset = useCallback(() => setItem(initial), [initial]);

    return (
      <ProjectItem
        className="edit-project"
        loading={loading}
        item={item}
        setItem={setItem}
        onSave={onSave}
        onReset={onReset}
        onRemove={onRemove}
        onDocumentDonwloaded={onDocumentDonwloaded}
        onUserStoryEditorCLick={onUserStoryEditorCLick}
      />
    );
  }
);
