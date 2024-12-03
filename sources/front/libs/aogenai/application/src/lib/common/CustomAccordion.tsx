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
  open?: boolean;
  onChange?: (expanded: boolean) => void;
}
export const CustomAccordion: FC<PropsWithChildren<ICustomAccordionProps>> =
  memo(({ title, open, children, onChange }) => (
    <StyledAccordion
      expanded={open}
      onChange={(_, expanded) => onChange?.(expanded)}
    >
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
  '& .MuiAccordionSummary-root': {
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
