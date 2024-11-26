import { IProjectDto } from '@aogenai/domain';
import { TextField } from '@mui/material';
import { FC, memo } from 'react';
import { onPropertyChange } from '../../tools';

interface ICreateContentProps {
  data: IProjectDto;
  setData: (data: IProjectDto) => void;
}

export const CreateContent: FC<ICreateContentProps> = memo(
  ({ data, setData }) => {
    return (
      <TextField
        label="Name"
        name="Name"
        value={data.name}
        onChange={onPropertyChange({ property: 'name', data, setData })}
        variant="outlined"
      />
    );
  }
);
