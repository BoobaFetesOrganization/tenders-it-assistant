import { IUserStoryPromptDto } from '@aogenai/domain';
import { Grid2, TextField } from '@mui/material';
import { FC, memo } from 'react';
import { onPropertyChange } from '../../../tools';

interface IRequestProps {
  request: IUserStoryPromptDto;
  onChanged: (request: IUserStoryPromptDto) => void;
}
export const UserGroupRequest: FC<IRequestProps> = memo(
  ({ request, onChanged }) => {
    return (
      <Grid2 container flex={1} direction="column" gap={2}>
        <TextField
          label="Context"
          value={request.context}
          onChange={onPropertyChange({
            item: request,
            setItem: onChanged,
            property: 'context',
          })}
          multiline
          fullWidth
        />
        <TextField
          label="Personas"
          value={request.personas}
          onChange={onPropertyChange({
            item: request,
            setItem: onChanged,
            property: 'personas',
          })}
          variant="outlined"
          multiline
          fullWidth
        />
        <TextField
          label="Tasks"
          value={request.tasks}
          onChange={onPropertyChange({
            item: request,
            setItem: onChanged,
            property: 'tasks',
          })}
          variant="outlined"
          multiline
          fullWidth
        />
      </Grid2>
    );
  }
);
