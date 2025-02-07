import { styled, TableCell } from '@mui/material';

export const DataHead = styled(TableCell)(({ theme }) => ({
  '& > .MuiTypography-root': {
    fontWeight: 'bold',
  },
}));
