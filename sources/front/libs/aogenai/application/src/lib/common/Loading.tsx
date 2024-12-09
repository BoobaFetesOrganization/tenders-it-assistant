import { Box, BoxProps, CircularProgress, Fade, useTheme } from '@mui/material';
import { FC, memo } from 'react';

const defaultProps: BoxProps['sx'] = {
  display: 'flex',
  gap: 2,
};

interface ILoadingProps {
  showImmediately?: boolean;
}
export const Loading: FC<ILoadingProps> = memo(({ showImmediately }) => {
  const theme = useTheme();
  return (
    <Box
      sx={{
        ...defaultProps,
        alignItems: 'start',
        justifyItems: 'start',
        flexGrow: 1,
      }}
    >
      <Fade
        in
        style={{
          transitionDelay: showImmediately ? '0s' : '2s',
        }}
        unmountOnExit
      >
        <Box
          sx={{
            ...defaultProps,
            alignItems: 'center',
          }}
        >
          <CircularProgress size={theme.spacing(2)} />
          Loading...
        </Box>
      </Fade>
    </Box>
  );
});
