import { newUserStoryGroupDto } from '@aogenai/domain';
import { useUserStoryGroup } from '@aogenai/infra';
import { Box, Grid2, List, TextField, Typography } from '@mui/material';
import { FC, memo, ReactNode, useState } from 'react';
import { CustomAccordion, useTotalCost } from '../common';
import { Story } from './component/Story';

interface IUserStoryGroupProps {
  projectId?: number;
  groupId?: number;
  showRequest?: boolean;
  actions?: ReactNode;
}

export const UserStoryGroup: FC<IUserStoryGroupProps> = memo(
  ({ projectId = 0, groupId = 0, showRequest, actions }) => {
    const [open, setOpen] = useState(false);

    const { data: { group } = { group: newUserStoryGroupDto() } } =
      useUserStoryGroup({
        variables: { projectId, id: groupId },
      });

    const total = useTotalCost(group);

    return (
      <Grid2 container flex={1} direction="column" gap={2}>
        <Grid2 container>
          <Grid2 container flex={1}>
            <Box>{actions}</Box>
          </Grid2>
          <Grid2
            container
            flex={1}
            sx={{
              border: '1px solid',
              borderColor: 'grey.400',
              borderRadius: 1,
              padding: 2,
              margin: 2,
              width: '50%',
              marginLeft: 'auto',
              position: 'relative',
              backgroundColor: 'white',
              display: 'flex',
              flexDirection: 'column',
            }}
          >
            <Box
              sx={{
                position: 'absolute',
                top: '-16px',
                left: '16px',
                backgroundColor: 'white',
                padding: '0 8px',
              }}
            >
              <Typography variant="h6">Legend</Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Typography variant="subtitle1">Cost:</Typography>
              <Typography variant="subtitle1">
                <b>{total.cost}</b>
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Typography variant="subtitle1">User stories count:</Typography>
              <Typography variant="subtitle1">
                {group.userStories.length}
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Typography variant="subtitle1">Tasks count:</Typography>
              <Typography variant="subtitle1">
                {group.userStories.reduce(
                  (acc, story) => acc + story.tasks.length,
                  0
                )}
              </Typography>
            </Box>
          </Grid2>
        </Grid2>
        {showRequest && (
          <Grid2>
            <CustomAccordion title="Request" open={open} onChange={setOpen}>
              <Grid2 container flex={1} direction="column" gap={2}>
                <TextField
                  label="Context"
                  value={group.request.context}
                  multiline
                  slotProps={{ input: { readOnly: true } }}
                  fullWidth
                />
                <TextField
                  label="Personas"
                  value={group.request.personas}
                  variant="outlined"
                  multiline
                  slotProps={{ input: { readOnly: true } }}
                  fullWidth
                />
                <TextField
                  label="Tasks"
                  value={group.request.tasks}
                  variant="outlined"
                  multiline
                  slotProps={{ input: { readOnly: true } }}
                  fullWidth
                />
              </Grid2>
            </CustomAccordion>
          </Grid2>
        )}
        <Grid2>
          <List>
            {group.userStories.map((us) => (
              <Story key={us.id} {...us} />
            ))}
          </List>
        </Grid2>
      </Grid2>
    );
  }
);
