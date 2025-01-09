import { Grid2, styled } from '@mui/material';

export const DataContent = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  flexDirection: 'column',
  overflow: 'hidden',
  '& table > tbody> tr > td': { cursor: 'context-menu' },
}));
