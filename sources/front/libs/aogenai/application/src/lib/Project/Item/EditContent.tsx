import { IProjectDto } from '@aogenai/domain';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  styled,
  TextField,
  Typography,
} from '@mui/material';
import { FC, memo } from 'react';
import { DocumentCollection } from '../../Document';
import { onPropertyChange } from '../../tools';

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
      <StyledAccordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="document-collection-content"
          id="document-collection-header"
        >
          <Typography>Documents</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <DocumentCollection projectId={data.id} />
        </AccordionDetails>
      </StyledAccordion>
      <TextField
        label="Prompt"
        name="prompt"
        value={data.prompt}
        onChange={onPropertyChange({ property: 'prompt', data, setData })}
        variant="outlined"
        multiline
        rows={20}
        fullWidth
      />
      <TextField
        label="Response ID"
        name="responseId"
        type="number"
        value={data.responseId}
        onChange={onPropertyChange({
          property: 'responseId',
          data,
          setData,
          getValue: (val) => +val,
        })}
        variant="outlined"
      />
    </Box>
  );
});

const StyledAccordion = styled(Accordion)(({ theme }) => ({
  borderRadius: '5px',
  '& .MuiButtonBase-root': {
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.primary.contrastText,
  },
  '& .MuiAccordionSummary-expandIconWrapper': {
    color: theme.palette.primary.contrastText,
  },
  '&.MuiAccordion-root::before': {
    display: 'none',
  },
}));
