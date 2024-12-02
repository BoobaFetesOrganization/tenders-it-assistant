import { Box, BoxProps, CircularProgress, Fade } from '@mui/material';
import { FC, memo } from 'react';

const defaultProps: BoxProps['sx'] = {
  display: 'flex',
  gap: 2,
};

export const Loading: FC = memo(() => {
  return (
    <Box sx={{ ...defaultProps, alignItems: 'start', flexGrow: 1 }}>
      <Fade
        in
        style={{
          transitionDelay: '2s',
        }}
        unmountOnExit
      >
        <Box
          sx={{
            ...defaultProps,
            alignItems: 'center',
          }}
        >
          <CircularProgress />
          Loading...
        </Box>
      </Fade>
    </Box>
  );
});
