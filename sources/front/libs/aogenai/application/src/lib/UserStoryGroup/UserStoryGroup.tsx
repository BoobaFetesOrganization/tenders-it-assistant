import { newUserStoryGroupDto } from '@aogenai/domain';
import { useUserStoryGroup } from '@aogenai/infra';
import { Grid2, List, TextField } from '@mui/material';
import { FC, memo, useState } from 'react';
import { CustomAccordion, CustomForm, ICustomFormProps } from '../common';
import { Story } from './component/Story';

interface IUserStoryGroupProps extends ICustomFormProps {
  projectId: number;
  groupId: number;
  showRequest?: boolean;
}

export const UserStoryGroup: FC<IUserStoryGroupProps> = memo(
  ({ projectId, groupId, showRequest = true, ...formProps }) => {
    const [open, setOpen] = useState(true);

    const { data: { group } = { group: newUserStoryGroupDto() } } =
      useUserStoryGroup({
        variables: { projectId, id: groupId },
      });

    return (
      <CustomForm {...formProps}>
        <Grid2 container flex={1} direction="column" gap={2}>
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
      </CustomForm>
    );
  }
);
