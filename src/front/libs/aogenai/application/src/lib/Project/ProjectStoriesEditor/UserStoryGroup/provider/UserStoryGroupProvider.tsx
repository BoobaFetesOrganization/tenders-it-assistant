import { IUserStoryGroupDto, newUserStoryGroupDto } from '@aogenai/domain';
import { useUserStoryGroup } from '@aogenai/infra';
import {
  createContext,
  Dispatch,
  FC,
  memo,
  PropsWithChildren,
  useContext,
  useReducer,
  useState,
} from 'react';
import { Action, initGroup, reducer } from './data';

interface IUserStoryGroupContext {
  initial: IUserStoryGroupDto;
  group: IUserStoryGroupDto;
  dispatch: Dispatch<Action>;
  onDeleted?: (group: IUserStoryGroupDto) => void;
}

const UserStoryGroupContext = createContext<IUserStoryGroupContext>(
  undefined as unknown as IUserStoryGroupContext
);

export const useUserStoryGroupContext = () => useContext(UserStoryGroupContext);

export interface IUserStoryGroupProviderProps {
  projectId: number;
  groupId: number;
  onDeleted?: (group: IUserStoryGroupDto) => void;
}
export const UserStoryGroupProvider: FC<
  PropsWithChildren<IUserStoryGroupProviderProps>
> = memo(({ projectId, groupId: id, onDeleted, children }) => {
  const [initial, setInitial] = useState(newUserStoryGroupDto());
  const [group, dispatch] = useReducer(reducer, initial);

  useUserStoryGroup({
    variables: { projectId, id },
    onCompleted: ({ group }) => {
      setInitial(group);
      initGroup(dispatch, group);
    },
  });

  return (
    <UserStoryGroupContext.Provider
      value={{ group, initial, dispatch, onDeleted }}
    >
      {children}
    </UserStoryGroupContext.Provider>
  );
});
