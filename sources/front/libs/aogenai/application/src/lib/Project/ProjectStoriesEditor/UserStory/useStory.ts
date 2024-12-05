import { ITaskCostDto, TaskCostKind } from '@aogenai/domain';
import { useMemo } from 'react';
import { createTask, deleteTask, updateStory, updateTask } from './data';
import { useStoryContext } from './StoryProvider';

export const useStory = () => {
  const context = useStoryContext();
  if (context === undefined) {
    throw new Error('useStory must be used within a StoryProvider');
  }

  const { dispatch, story } = context;
  return useMemo(
    () => ({
      story,
      updateStory: updateStory(dispatch),
      createTask: createTask(dispatch),
      updateTask: updateTask(dispatch),
      deleteTask: deleteTask(dispatch),
      upsertWorkingCost: (index: number, taskCost: ITaskCostDto) => {
        const update = updateTask(dispatch);
        const task = story.tasks[index];
        if (!task) return;

        if (taskCost.kind !== TaskCostKind.Human)
          throw new Error(
            'Invalid task cost kind, only human is allowed to update costs'
          );

        const hasHumanCost = task.workingCosts.some(
          (i) => i.kind === TaskCostKind.Human
        );
        update({
          index,
          entity: {
            ...task,
            workingCosts: !hasHumanCost
              ? [...task.workingCosts, taskCost]
              : task.workingCosts.map((item) =>
                  item.id === taskCost.id ? taskCost : item
                ),
          },
        });
      },
    }),
    [story, dispatch]
  );
};
