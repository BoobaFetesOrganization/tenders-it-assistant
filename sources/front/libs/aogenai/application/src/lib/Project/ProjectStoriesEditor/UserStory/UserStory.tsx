import { IUserStoryDto } from '@aogenai/domain';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import { Grid2, IconButton, Paper, TextField, Theme } from '@mui/material';
import { FC, memo, useCallback, useMemo } from 'react';
import { onPropertyChange } from '../../../tools';
import { useUserStoryGroupData } from '../UserStoryGroup';
import { Task } from './Task';

interface IUserStoryProps {
  storyIndex: number;
}
export const UserStory: FC<IUserStoryProps> = memo(({ storyIndex }) => {
  const { story, task } = useUserStoryGroupData();

  const { item, setItem } = useMemo(
    () => ({
      item: story.get(storyIndex),
      setItem: (entity: IUserStoryDto) => story.update({ storyIndex, entity }),
    }),
    [story, storyIndex]
  );

  const onRemoveStory = useCallback(() => {
    story.delete({ storyIndex });
  }, [story, storyIndex]);

  const onAddTask = useCallback(() => {
    task.create({ storyIndex });
  }, [task, storyIndex]);

  return (
    <Grid2 container direction="column" id="userstory-editor" {...rootProps}>
      <Grid2 container flex={1} spacing={2} flexWrap="nowrap">
        <Grid2>
          <IconButton color="error" onClick={onRemoveStory}>
            <RemoveIcon />
          </IconButton>
        </Grid2>
        <Grid2 flex={1}>
          <TextField
            label="Name"
            size="small"
            value={item.name}
            onChange={onPropertyChange({ item, setItem, property: 'name' })}
            fullWidth
          />
        </Grid2>
        <Grid2>
          <TextField
            label="Cost"
            size="small"
            value={!item.cost ? '' : item.cost}
            slotProps={{ input: { readOnly: true } }}
            sx={{ width: 75 }}
          />
        </Grid2>
      </Grid2>
      <Grid2
        container
        direction="column"
        flex={1}
        spacing={2}
        sx={{
          margin: (theme) => theme.spacing(2, 0, 2, 2),
          padding: (theme) => theme.spacing(0, 0, 0, 2),
          borderLeftWidth: 1,
          borderLeftStyle: 'solid',
          borderLeftColor: 'divider',
        }}
      >
        {task.list(storyIndex).map(({ id }, taskIndex) => (
          <Task
            key={`${storyIndex}-${taskIndex}-${id}-`}
            storyIndex={storyIndex}
            taskIndex={taskIndex}
          />
        ))}
        <Grid2 container justifyContent="start">
          <IconButton color="primary" onClick={onAddTask}>
            <AddIcon />
          </IconButton>
        </Grid2>
      </Grid2>
    </Grid2>
  );
});

const rootProps = {
  component: Paper,
  sx: { padding: (theme: Theme) => theme.spacing(1) },
};
