import { FC, memo } from 'react';
import { IUserStoryProps } from './IUserStoryProps';
import { StoryProvider } from './StoryProvider';
import { UserStory as UserStoryInternal } from './UserStory';

export const UserStory: FC<IUserStoryProps> = memo((props) => {
  return (
    <StoryProvider {...props}>
      <UserStoryInternal />
    </StoryProvider>
  );
});
