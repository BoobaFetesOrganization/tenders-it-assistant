import { IUserStoryGroupDto, newUserStoryGroupDto } from '@aogenai/domain';
import {
  useDeleteUserStoryGroup,
  useUpdateUserStoryGroup,
  useUserStoryGroup,
} from '@aogenai/infra';
import {
  FC,
  HTMLAttributes,
  memo,
  ReactNode,
  useCallback,
  useState,
} from 'react';
import { Loading } from '../common';
import { UserStoryGroupItem } from './Item';

interface IUserStoryGroupEditProps extends HTMLAttributes<HTMLElement> {
  projectId: number;
  groupId: number;
  onSaved?: (item: IUserStoryGroupDto) => void;
  onDeleted?: (item: IUserStoryGroupDto) => void;
  actions?: ReactNode;
}
export const UserStoryGroupEdit: FC<IUserStoryGroupEditProps> = memo(
  ({ projectId, groupId, onSaved, onDeleted, actions, ...htmlAttr }) => {
    const [initial, setInitial] = useState(newUserStoryGroupDto);

    const { loading } = useUserStoryGroup({
      variables: { projectId, id: groupId },
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
      <Loading />
    ) : (
      <UserStoryGroupItem
        {...htmlAttr}
        actions={actions}
        className="edit-group"
        data={initial}
        reset={reset}
        save={save}
        remove={remove}
      />
    );
  }
);
