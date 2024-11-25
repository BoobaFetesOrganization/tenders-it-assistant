import { createTheme } from '@mui/material';

export const theme = createTheme({
  components: {
    MuiCssBaseline: {
      styleOverrides: (theme) => ({
        body: {
          '*::-webkit-scrollbar': {
            width: '6px',
          },
          '*::-webkit-scrollbar-track': {
            background: 'transparent',
          },
          '*::-webkit-scrollbar-thumb': {
            backgroundColor: theme.palette.primary.main,
            borderRadius: 5,
            border: `3px solid ${theme.palette.primary.main}`,
            bordersize: 1,
          },
          '*::-webkit-scrollbar-thumb:hover': {
            background: theme.palette.primary.main,
            cursor: 'grab',
          },
        },
      }),
    },
  },
});
