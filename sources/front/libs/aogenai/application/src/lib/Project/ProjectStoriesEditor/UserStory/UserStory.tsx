import AddIcon from '@mui/icons-material/Add';
import { Grid2, IconButton, TextField } from '@mui/material';
import { FC, memo } from 'react';
import { onPropertyChange } from '../../../tools';
import { Task } from './Task';
import { useStory } from './useStory';

export const UserStory: FC = memo(() => {
  const { story, updateStory, createTask } = useStory();

  return (
    <Grid2 container direction="column" id="userstory-editor">
      <Grid2 container flex={1} spacing={2} flexWrap="nowrap">
        <TextField
          label="Name"
          value={story.name}
          onChange={onPropertyChange({
            item: story,
            setItem: updateStory,
            property: 'name',
          })}
          fullWidth
        />
        <TextField
          label="Cost"
          value={story.cost}
          onChange={onPropertyChange({
            item: story,
            setItem: updateStory,
            property: 'cost',
            getValue: parseFloat,
          })}
          sx={{ width: 75 }}
        />
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
        {story.tasks.map((task, index) => (
          <Task key={`${task.id}-${index}`} {...task} index={index} />
        ))}
        <Grid2 container justifyContent="start">
          <IconButton color="primary" onClick={createTask}>
            <AddIcon />
          </IconButton>
        </Grid2>
      </Grid2>
    </Grid2>
  );
});
