import { Grid2, styled } from '@mui/material';

export const Root = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  flexDirection: 'column',
}));
