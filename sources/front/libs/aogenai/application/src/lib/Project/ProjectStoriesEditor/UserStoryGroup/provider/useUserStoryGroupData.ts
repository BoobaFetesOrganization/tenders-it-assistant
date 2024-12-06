import { useMemo } from 'react';
import {
  createStory,
  createTask,
  deleteStory,
  deleteTask,
  updateRequest,
  updateStory,
  updateTask,
} from './data';
import { useUserStoryGroupContext } from './UserStoryGroupProvider';

export const useUserStoryGroupData = () => {
  const { group, dispatch, onDeleted } = useUserStoryGroupContext();

  return useMemo(() => {
    return {
      onDeleted,
      group,
      request: {
        data: group.request,
        update: updateRequest(dispatch),
      },
      story: {
        list: () => group.userStories,
        get: (storyIndex: number) => group.userStories[storyIndex],
        create: createStory(dispatch),
        update: updateStory(dispatch),
        delete: deleteStory(dispatch),
      },
      task: {
        list: (storyIndex: number) => group.userStories[storyIndex].tasks,
        get: (storyIndex: number, taskIndex: number) =>
          group.userStories[storyIndex].tasks[taskIndex],
        create: createTask(dispatch),
        update: updateTask(dispatch),
        delete: deleteTask(dispatch),
      },
    };
  }, [onDeleted, group, dispatch]);
};
