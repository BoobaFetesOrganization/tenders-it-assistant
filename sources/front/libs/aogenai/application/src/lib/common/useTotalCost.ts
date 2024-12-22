import { IUserStoryGroupDto, TaskCostKind } from '@aogenai/domain';
import { useMemo } from 'react';

export const useTotalCost = ({ userStories }: IUserStoryGroupDto) => {
  return useMemo(() => {
    return {
      cost: userStories.reduce((acc, cv) => acc + cv.cost || 0, 0),
      geminiCost: userStories.reduce((acc, cv) => {
        for (const task of cv.tasks || []) {
          const gemini = task?.workingCosts.find(
            (wc) => wc.kind === TaskCostKind.Gemini
          );
          if (gemini) acc += gemini.cost || 0;
        }
        return acc;
      }, 0),
    };
  }, [userStories]);
};
