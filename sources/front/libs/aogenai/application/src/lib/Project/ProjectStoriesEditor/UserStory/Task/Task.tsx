import { ITaskDto, TaskCostKind } from '@aogenai/domain';
import RemoveIcon from '@mui/icons-material/Remove';
import { Grid2, IconButton, TextField } from '@mui/material';
import { FC, memo, useCallback, useMemo } from 'react';
import { onPropertyChange } from '../../../../tools';
import { useUserStoryGroupData } from '../../UserStoryGroup';

interface ITaskProps {
  storyIndex: number;
  taskIndex: number;
}
export const Task: FC<ITaskProps> = memo(({ storyIndex, taskIndex }) => {
  const { task } = useUserStoryGroupData();

  const { item, setItem, gemini } = useMemo(() => {
    const item = task.get(storyIndex, taskIndex);
    return {
      item,
      setItem: (entity: ITaskDto) =>
        task.update({ storyIndex, taskIndex, entity }),
      gemini: item.workingCosts?.find(
        (cost) => cost.kind === TaskCostKind.Gemini
      ),
    };
  }, [storyIndex, task, taskIndex]);

  const onDelete = useCallback(() => {
    task.delete({ storyIndex, taskIndex });
  }, [storyIndex, task, taskIndex]);

  return (
    <Grid2
      container
      direction="column"
      flex={1}
      spacing={1}
      sx={{ padding: (theme) => theme.spacing(0, 0, 1, 0) }}
    >
      <Grid2 container flex={1} spacing={2}>
        <Grid2>
          <IconButton color="error" onClick={onDelete}>
            <RemoveIcon />
          </IconButton>
        </Grid2>
        <Grid2 flex={1}>
          <TextField
            size="small"
            label="Name"
            value={item.name}
            onChange={onPropertyChange({ item, setItem, property: 'name' })}
            fullWidth
          />
        </Grid2>
        <Grid2>
          <TextField
            size="small"
            label="Cost"
            value={item.cost ?? 0}
            onChange={onPropertyChange({
              item,
              setItem,
              property: 'cost',
              getValue: parseFloat,
            })}
            sx={{ width: 75 }}
          />
        </Grid2>
        {gemini && (
          <Grid2>
            <TextField
              size="small"
              label="Gemini"
              className="Mui-focused"
              color="secondary"
              focused
              value={gemini.cost ?? 0}
              slotProps={{ input: { readOnly: true } }}
              sx={{ width: 80 }}
            />
          </Grid2>
        )}
      </Grid2>
    </Grid2>
  );
});
