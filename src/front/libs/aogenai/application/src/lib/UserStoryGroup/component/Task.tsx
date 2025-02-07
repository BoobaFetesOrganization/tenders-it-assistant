import { ITaskDto } from '@aogenai/domain';
import TaskIcon from '@mui/icons-material/TaskAlt';
import { Box, ListItem, ListItemIcon, TextField } from '@mui/material';
import { FC, memo } from 'react';

export const Task: FC<ITaskDto> = memo(({ id, name, cost, workingCosts }) => {
  return (
    <ListItem sx={{ pl: 4 }}>
      <ListItemIcon>
        <TaskIcon />
      </ListItemIcon>
      <Box sx={{ display: 'flex', flex: 1, gap: 2 }}>
        <TextField
          label="Story Name"
          size="small"
          value={name}
          slotProps={{ input: { readOnly: true } }}
          autoFocus
          fullWidth
        />
        <TextField
          label="Cost"
          size="small"
          value={cost}
          slotProps={{ input: { readOnly: true } }}
          autoFocus
          fullWidth={false}
        />
      </Box>
    </ListItem>
  );
});
