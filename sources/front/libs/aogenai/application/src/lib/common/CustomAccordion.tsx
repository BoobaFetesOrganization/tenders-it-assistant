import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  styled,
  Typography,
} from '@mui/material';
import { FC, memo, PropsWithChildren } from 'react';

interface ICustomAccordionProps {
  title: string;
}
export const CustomAccordion: FC<PropsWithChildren<ICustomAccordionProps>> =
  memo(({ title, children }) => (
    <StyledAccordion>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="document-collection-content"
        id="document-collection-header"
      >
        <Typography>{title}</Typography>
      </AccordionSummary>
      <AccordionDetails>{children}</AccordionDetails>
    </StyledAccordion>
  ));

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
