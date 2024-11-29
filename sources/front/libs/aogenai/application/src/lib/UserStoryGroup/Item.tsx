import { IUserStoryGroupDto } from '@aogenai/domain';
import { Box, Button, TextField } from '@mui/material';
import { FC, memo, useCallback, useEffect, useState } from 'react';
import { CustomAccordion } from '../common';

interface dataProps {
  className?: string;
  data: IUserStoryGroupDto;
  save: (data: IUserStoryGroupDto) => void;
  reset: () => IUserStoryGroupDto;
  remove?: (item: IUserStoryGroupDto) => void;
}

export const UserStoryGroupItem: FC<dataProps> = memo(
  ({ className, data, save, reset, remove }) => {
    const [item, setItem] = useState<IUserStoryGroupDto>(data);

    useEffect(() => {
      setItem(data);
    }, [data]);

    const onSave = useCallback(() => {
      save(item);
    }, [item, save]);

    const onReset = useCallback(() => {
      setItem(reset());
    }, [reset]);

    const onDelete = useCallback(() => {
      remove?.(data);
    }, [data, remove]);

    return (
      <Box
        className={className}
        sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
      >
        <p>table for userstories and tasks but not the cost rom human or IA</p>
        <CustomAccordion title="Request">
          <TextField
            label="Context"
            value={data.request.context}
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
            value={data.request.personas}
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
            value={data.request.tasks}
            onChange={({ target: { value: tasks } }) =>
              setItem({ ...item, request: { ...item.request, tasks } })
            }
            variant="outlined"
            multiline
            rows={20}
            fullWidth
          />
        </CustomAccordion>
        <Button variant="outlined" color="error" onClick={onDelete}>
          Delete
        </Button>
        <Button variant="outlined" color="secondary" onClick={onReset}>
          Reset
        </Button>
        <Button variant="contained" color="primary" onClick={onSave}>
          Save
        </Button>
      </Box>
    );
  }
);
