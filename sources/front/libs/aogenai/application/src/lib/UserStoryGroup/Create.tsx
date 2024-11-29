import { IUserStoryGroupDto, newUserStoryGroupDto } from '@aogenai/domain';
import { useCreateUserStoryGroup } from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { UserStoryGroupItem } from './Item';

interface IUserStoryGroupCreateProps {
  projectId: number;
  onCreated: (item: IUserStoryGroupDto) => void;
}
export const UserStoryGroupCreate: FC<IUserStoryGroupCreateProps> = memo(
  ({ projectId, onCreated }) => {
    const [intial, setInitial] = useState(newUserStoryGroupDto);

    const [call] = useCreateUserStoryGroup({
      onCompleted({ group }) {
        if (group.id) onCreated(group);
        else setInitial(group);
      },
    });

    const save = useCallback(
      (data: IUserStoryGroupDto) => {
        call({ variables: { projectId, input: data } });
      },
      [call, projectId]
    );

    return (
      <UserStoryGroupItem
        className="create-group"
        data={intial}
        reset={newUserStoryGroupDto}
        save={save}
      />
    );
  }
);
