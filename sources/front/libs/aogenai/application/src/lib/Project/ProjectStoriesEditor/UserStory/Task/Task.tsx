import { ITaskDto, newTaskCostDto, TaskCostKind } from '@aogenai/domain';
import RemoveIcon from '@mui/icons-material/Remove';
import { Grid2, IconButton, TextField } from '@mui/material';
import { FC, memo, useCallback, useMemo } from 'react';
import { onPropertyChange } from '../../../../tools';
import { useStory } from '../useStory';

export const Task: FC<{ index: number } & ITaskDto> = memo((task, index) => {
  const { deleteTask, updateTask, upsertWorkingCost } = useStory();

  const workingCosts = useMemo(() => {
    const humanCostIndex = task.workingCosts?.findIndex(
      (cost) => cost.kind === TaskCostKind.Human
    );

    return {
      Gemini: task.workingCosts?.find(
        (cost) => cost.kind === TaskCostKind.Gemini
      ),
      Human:
        humanCostIndex !== -1
          ? task.workingCosts[humanCostIndex]
          : newTaskCostDto(),
    };
  }, [task.workingCosts]);

  const onDelete = useCallback(() => {
    deleteTask(task.id);
  }, [deleteTask, task.id]);

  const setItem = useCallback(
    (entity: ITaskDto) => updateTask({ index, entity }),
    [updateTask, index]
  );

  return (
    <Grid2
      container
      direction="column"
      flex={1}
      spacing={1}
      sx={{ padding: (theme) => theme.spacing(0, 0, 1, 0) }}
    >
      <Grid2 container flex={1} spacing={2} flexWrap="nowrap">
        <IconButton color="error" onClick={onDelete}>
          <RemoveIcon />
        </IconButton>
        <TextField
          size="small"
          label="Name"
          value={task.name}
          onChange={onPropertyChange({
            item: task,
            setItem,
            property: 'name',
          })}
          fullWidth
        />
        <TextField
          size="small"
          label="Cost"
          value={task.cost}
          onChange={onPropertyChange({
            item: task,
            setItem,
            property: 'cost',
            getValue: parseFloat,
          })}
          sx={{ width: 75 }}
        />
      </Grid2>
      <Grid2 container flex={1} spacing={2} justifyContent="end">
        <TextField
          size="small"
          label="Gemini"
          className="Mui-focused"
          color="secondary"
          focused
          contentEditable={false}
          value={workingCosts.Gemini?.cost ?? 0}
          sx={{ width: 80 }}
        />
        <TextField
          size="small"
          label="Human"
          value={workingCosts.Human?.cost ?? 0}
          onChange={onPropertyChange({
            item: workingCosts.Human,
            setItem: (item) => upsertWorkingCost(index, item),
            property: 'cost',
            getValue: parseFloat,
          })}
          sx={{ width: 80 }}
        />
      </Grid2>
    </Grid2>
  );
});
