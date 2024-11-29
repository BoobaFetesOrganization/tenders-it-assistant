import { IUserStoryGroupDto, newUserStoryGroupDto } from '@aogenai/domain';
import {
  useDeleteUserStoryGroup,
  useUpdateUserStoryGroup,
  useUserStoryGroup,
} from '@aogenai/infra';
import { FC, memo, useCallback, useState } from 'react';
import { UserStoryGroupItem } from './Item';

interface IEditProps {
  projectId: number;
  id: number;
  onSaved?: (item: IUserStoryGroupDto) => void;
  onDeleted?: (item: IUserStoryGroupDto) => void;
}
export const UserStoryGroupEdit: FC<IEditProps> = memo(
  ({ projectId, id, onSaved, onDeleted }) => {
    const [initial, setInitial] = useState(newUserStoryGroupDto);

    const { loading } = useUserStoryGroup({
      variables: { projectId, id },
      onCompleted({ group }) {
        setInitial(group);
      },
    });

    const [call] = useUpdateUserStoryGroup({
      onCompleted({ group }) {
        alert(`UserStoryGroup updated`);
        onSaved?.(group);
      },
    });

    const [deleteUserStoryGroup] = useDeleteUserStoryGroup({
      onCompleted({ group }) {
        onDeleted?.(group);
      },
    });

    const save = useCallback(
      (data: IUserStoryGroupDto) => {
        call({ variables: { projectId, input: data } });
      },
      [call, projectId]
    );

    const remove = useCallback(
      (data: IUserStoryGroupDto) => {
        deleteUserStoryGroup({ variables: { projectId, id: data.id } });
      },
      [deleteUserStoryGroup, projectId]
    );

    const reset = useCallback(() => initial, [initial]);

    return loading ? (
      'is loading'
    ) : (
      <UserStoryGroupItem
        className="edit-group"
        data={initial}
        reset={reset}
        save={save}
        remove={remove}
      />
    );
  }
);
