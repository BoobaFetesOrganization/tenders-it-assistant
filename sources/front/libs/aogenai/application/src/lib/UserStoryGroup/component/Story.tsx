import { IUserStoryDto } from '@aogenai/domain';
import UserStoryIcon from '@mui/icons-material/FormatListNumbered';
import {
  Box,
  Collapse,
  List,
  ListItem,
  ListItemIcon,
  TextField,
} from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import { Task } from './Task';

export const Story: FC<IUserStoryDto> = memo(({ id, name, cost, tasks }) => {
  const [open, setOpen] = useState(true);
  const toggleOpen = useCallback(() => setOpen(!open), [open]);
  return (
    <>
      <ListItem key={id}>
        <ListItemIcon>
          <UserStoryIcon />
        </ListItemIcon>
        <Box sx={{ display: 'flex', flex: 1, gap: 2 }}>
          <TextField
            label="Story Name"
            value={name}
            slotProps={{ input: { readOnly: true } }}
            autoFocus
            fullWidth
          />
          <TextField
            label="Cost"
            value={cost}
            slotProps={{ input: { readOnly: true } }}
            autoFocus
            fullWidth={false}
          />
        </Box>
      </ListItem>
      <Collapse in={open} onChange={toggleOpen} timeout="auto" unmountOnExit>
        <List component="div" disablePadding>
          {tasks.map((task) => (
            <Task key={task.id} {...task} />
          ))}
        </List>
      </Collapse>
    </>
  );
});
