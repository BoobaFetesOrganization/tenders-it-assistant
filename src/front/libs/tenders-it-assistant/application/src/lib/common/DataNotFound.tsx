import { Box } from '@mui/material';
import { FC, memo } from 'react';

export const DataNotFound: FC = memo(() => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        height: '100%',
        gap: 2,
      }}
    >
      Data not found
    </Box>
  );
});
