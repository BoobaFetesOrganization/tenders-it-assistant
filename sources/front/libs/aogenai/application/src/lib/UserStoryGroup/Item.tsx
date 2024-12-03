import { IUserStoryGroupDto } from '@aogenai/domain';
import { TextField } from '@mui/material';
import { FC, HTMLAttributes, memo } from 'react';
import { CustomAccordion, FormWithButtons } from '../common';

interface IUserStoryGroupItemProps extends HTMLAttributes<HTMLDivElement> {
  data: IUserStoryGroupDto;
  save: (data: IUserStoryGroupDto) => void;
  reset: () => IUserStoryGroupDto;
  remove?: (item: IUserStoryGroupDto) => void;
}

export const UserStoryGroupItem: FC<IUserStoryGroupItemProps> = memo(
  ({ data, save, reset, remove, ...htmlAttributes }) => {
    return (
      <FormWithButtons
        {...htmlAttributes}
        data={data}
        save={save}
        reset={reset}
        remove={remove}
      >
        {(item, setItem) => (
          <>
            <p>
              table for userstories and tasks but not the cost rom human or IA
            </p>
            <CustomAccordion title="Request">
              <TextField
                label="Context"
                value={item.request.context}
                onChange={({ target: { value: context } }) =>
                  setItem({ ...item, request: { ...item.request, context } })
                }
                variant="outlined"
                multiline
                rows={20}
                fullWidth
              />
              <TextField
                label="Personas"
                value={item.request.personas}
                onChange={({ target: { value: personas } }) =>
                  setItem({ ...item, request: { ...item.request, personas } })
                }
                variant="outlined"
                multiline
                rows={20}
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
                rows={20}
                fullWidth
              />
            </CustomAccordion>
          </>
        )}
      </FormWithButtons>
    );
  }
);
