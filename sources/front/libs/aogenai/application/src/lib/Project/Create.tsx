import { IProjectDto, newProjectDto } from '@aogenai/domain';
import { useCreateProject } from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { Item } from './Item';

export const Create: FC = memo(() => {
  // const navigate = useNavigate();

  const [intial] = useState(newProjectDto);

  const [call] = useCreateProject({
    onCompleted({ project }) {
      alert(`Project created : ${project.id} - ${project.name}`);
      alert(
        `createion termione donc on redirige vers l'edition : ${project.id} - ${project.name}`
      );
    },
  });

  const save = useCallback(
    (data: IProjectDto) => {
      call({ variables: { input: data } });
    },
    [call]
  );

  return <Item mode="create" data={intial} reset={newProjectDto} save={save} />;
});
