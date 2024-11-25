import { CssBaseline, ThemeProvider } from '@mui/material';
import Grid from '@mui/material/Grid2';
import { styled } from '@mui/material/styles';
import { memo } from 'react';
import { BrowserRouter } from 'react-router';
import { SearchAppBar } from './HeaderBar';
import { AppRoutes } from './routes';
import { theme } from './theme';

const StyledRoot = styled(Grid)(({ theme }) => ({
  height: 'inherit',
  display: 'flex',
  flexWrap: 'nowrap',
  flexDirection: 'column',
  padding: 0,
  margin: 0,
}));

const StyledBody = styled(Grid)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  flexWrap: 'nowrap',
  padding: theme.spacing(1),
  overflow: 'auto',
}));

export const App = memo(() => (
  <BrowserRouter>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <StyledRoot id="app" container spacing={0}>
        <Grid id="app-bar" flex={1} flexGrow={0}>
          <SearchAppBar title="Suivi des appels d'offre" />
        </Grid>
        <StyledBody id="app-body" spacing={1}>
          <AppRoutes />
        </StyledBody>
      </StyledRoot>
    </ThemeProvider>
  </BrowserRouter>
));
