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
} from 'react';
import { Loading } from '../../../../common';
import { Action, initGroup, reducer } from './data';

interface IUserStoryGroupContext {
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
  const [group, dispatch] = useReducer(reducer, newUserStoryGroupDto());

  const { loading, called } = useUserStoryGroup({
    variables: { projectId, id },
    onCompleted: ({ group }) => {
      initGroup(dispatch, group);
    },
  });

  return !called || loading ? (
    <Loading />
  ) : (
    <UserStoryGroupContext.Provider value={{ group, dispatch, onDeleted }}>
      {children}
    </UserStoryGroupContext.Provider>
  );
});
