import { IUserStoryGroupDto } from '@aogenai/domain';
import { Grid2, TextField } from '@mui/material';
import { FC, HTMLAttributes, memo, ReactNode, useState } from 'react';
import { CustomAccordion, FormWithButtons } from '../common';
import { UserStoryCollection } from '../UserStory/Collection';

interface IUserStoryGroupItemProps extends HTMLAttributes<HTMLDivElement> {
  data: IUserStoryGroupDto;
  save: (data: IUserStoryGroupDto) => void;
  reset: () => IUserStoryGroupDto;
  remove?: (item: IUserStoryGroupDto) => void;
  actions?: ReactNode;
}

export const UserStoryGroupItem: FC<IUserStoryGroupItemProps> = memo(
  ({ data, save, reset, remove, ...formProps }) => {
    const [open, setOpen] = useState(true);
    return (
      <FormWithButtons
        {...formProps}
        data={data}
        save={save}
        reset={reset}
        remove={remove}
      >
        {(item, setItem) => (
          <Grid2 container flex={1} direction="column" gap={2}>
            <Grid2>
              <CustomAccordion title="Request" open={open} onChange={setOpen}>
                <Grid2 container flex={1} direction="column" gap={2}>
                  <TextField
                    label="Context"
                    value={item.request.context}
                    onChange={({ target: { value: context } }) =>
                      setItem({
                        ...item,
                        request: { ...item.request, context },
                      })
                    }
                    multiline
                    fullWidth
                  />
                  <TextField
                    label="Personas"
                    value={item.request.personas}
                    onChange={({ target: { value: personas } }) =>
                      setItem({
                        ...item,
                        request: { ...item.request, personas },
                      })
                    }
                    variant="outlined"
                    multiline
                    fullWidth
                  />
                  <TextField
                    label="Tasks"
                    value={item.request.tasks}
                    onChange={({ target: { value: tasks } }) =>
                      setItem({ ...item, request: { ...item.request, tasks } })
                    }
                    variant="outlined"
                    multiline
                    fullWidth
                  />
                </Grid2>
              </CustomAccordion>
            </Grid2>
            <Grid2>
              <UserStoryCollection
                projectId={data.projectId}
                groupId={data.id}
              />
            </Grid2>
          </Grid2>
        )}
      </FormWithButtons>
    );
  }
);
