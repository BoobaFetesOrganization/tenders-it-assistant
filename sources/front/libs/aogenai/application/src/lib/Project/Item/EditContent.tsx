import { IProjectDto } from '@aogenai/domain';
import { Box, TextField } from '@mui/material';
import { FC, memo } from 'react';
import { CustomAccordion } from '../../common';
import { DocumentCollection } from '../../Document';
import { onPropertyChange } from '../../tools';
import { UserStoryGroupCollection } from '../../UserStoryGroup';

interface IEditContentProps {
  data: IProjectDto;
  setData: (data: IProjectDto) => void;
}

export const EditContent: FC<IEditContentProps> = memo(({ data, setData }) => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
      <TextField
        label="Name"
        name="Name"
        value={data.name}
        onChange={onPropertyChange({ property: 'name', data, setData })}
        variant="outlined"
      />

      <CustomAccordion title="Documents">
        <DocumentCollection projectId={data.id} />
      </CustomAccordion>
      <CustomAccordion title="User stories">
        <UserStoryGroupCollection projectId={data.id} />
      </CustomAccordion>
    </Box>
  );
});
