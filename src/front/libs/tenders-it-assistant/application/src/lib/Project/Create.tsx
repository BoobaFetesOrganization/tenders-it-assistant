import { IProjectDto, newProjectDto } from '@tenders-it-assistant/domain';
import { useCreateProject } from '@tenders-it-assistant/infra';
import { FC, memo, useCallback, useState } from 'react';
import { ProjectItem } from './Item';

interface IProjectCreateProps {
  onCreated: (item: IProjectDto) => void;
}
export const ProjectCreate: FC<IProjectCreateProps> = memo(({ onCreated }) => {
  const [initial, setInitial] = useState(newProjectDto());
  const [item, setItem] = useState(initial);

  const [call] = useCreateProject({
    onCompleted({ project }) {
      if (project.id) onCreated(project);
      else setInitial(project);
    },
  });

  const onSave = useCallback(() => {
    call({ variables: { input: item } });
  }, [call, item]);

  const onReset = useCallback(() => {
    setItem(initial);
  }, [initial]);

  return (
    <ProjectItem
      className="create-project"
      item={item}
      setItem={setItem}
      onSave={onSave}
      onReset={onReset}
    />
  );
});
