import { FC, memo } from 'react';
import { UserStoryGroup as UserStoryGroupInternal } from './UserStoryGroup';
import {
  IUserStoryGroupProviderProps,
  UserStoryGroupProvider,
} from './provider';

export * from './provider';
export const UserStoryGroup: FC<IUserStoryGroupProviderProps> = memo(
  (props) => (
    <UserStoryGroupProvider {...props}>
      <UserStoryGroupInternal />
    </UserStoryGroupProvider>
  )
);
