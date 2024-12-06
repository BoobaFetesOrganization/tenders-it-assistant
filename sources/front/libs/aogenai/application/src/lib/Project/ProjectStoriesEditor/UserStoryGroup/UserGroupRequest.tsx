import { Grid2, TextField } from '@mui/material';
import { FC, memo } from 'react';
import { onPropertyChange } from '../../../tools';
import { useUserStoryGroupData } from './provider';

export const UserGroupRequest: FC = memo(() => {
  const { request } = useUserStoryGroupData();
  return (
    <Grid2 container flex={1} direction="column" gap={2}>
      <TextField
        label="Context"
        value={request.data.context}
        onChange={onPropertyChange({
          item: request.data,
          setItem: request.update,
          property: 'context',
        })}
        multiline
        fullWidth
      />
      <TextField
        label="Personas"
        value={request.data.personas}
        onChange={onPropertyChange({
          item: request.data,
          setItem: request.update,
          property: 'personas',
        })}
        variant="outlined"
        multiline
        fullWidth
      />
      <TextField
        label="Tasks"
        value={request.data.tasks}
        onChange={onPropertyChange({
          item: request.data,
          setItem: request.update,
          property: 'tasks',
        })}
        variant="outlined"
        multiline
        fullWidth
      />
    </Grid2>
  );
});
