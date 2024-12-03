import { newPage } from '@aogenai/domain';
import { newPaginationParameter, useUserStories } from '@aogenai/infra';
import UserStoryIcon from '@mui/icons-material/FormatListNumbered';
import TaskIcon from '@mui/icons-material/TaskAlt';
import {
  Box,
  Collapse,
  List,
  ListItem,
  ListItemIcon,
  TextField,
} from '@mui/material';
import { FC, memo } from 'react';

interface IUserStoryCollectionProps {
  projectId: number;
  groupId: number;
}

export const UserStoryCollection: FC<IUserStoryCollectionProps> = memo(
  ({ projectId, groupId }) => {
    const { data: { stories } = { stories: newPage() } } = useUserStories({
      variables: { ...newPaginationParameter(), projectId, groupId },
    });

    return (
      <List>
        {stories.data.map((us) => (
          <>
            <ListItem key={us.id}>
              <ListItemIcon>
                <UserStoryIcon />
              </ListItemIcon>
              <Box sx={{ display: 'flex', flex: 1, gap: 2 }}>
                <TextField
                  label="Story Name"
                  value={us.name}
                  // onChange={(e) => setStoryName(e.target.value)}
                  // onBlur={() => handleSaveStory(us.id)}
                  autoFocus
                  fullWidth
                />
                <TextField
                  label="Cost"
                  value={us.cost}
                  // onChange={(e) => setStoryName(e.target.value)}
                  // onBlur={() => handleSaveStory(us.id)}
                  autoFocus
                  fullWidth={false}
                />
              </Box>
            </ListItem>
            <Collapse in timeout="auto" unmountOnExit>
              <List component="div" disablePadding>
                {us.tasks.map((task) => (
                  <ListItem sx={{ pl: 4 }}>
                    <ListItemIcon>
                      <TaskIcon />
                    </ListItemIcon>
                    <Box sx={{ display: 'flex', flex: 1, gap: 2 }}>
                      <TextField
                        label="Story Name"
                        value={task.name}
                        // onChange={(e) => setStoryName(e.target.value)}
                        // onBlur={() => handleSaveStory(us.id)}
                        autoFocus
                        fullWidth
                      />
                      <TextField
                        label="Cost"
                        value={task.cost}
                        // onChange={(e) => setStoryName(e.target.value)}
                        // onBlur={() => handleSaveStory(us.id)}
                        autoFocus
                        fullWidth={false}
                      />
                    </Box>
                  </ListItem>
                ))}
              </List>
            </Collapse>
          </>
        ))}
      </List>
    );
  }
);
