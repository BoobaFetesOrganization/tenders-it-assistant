//import { Search } from '@mui/icons-material';
import MenuIcon from '@mui/icons-material/Menu';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { FC, memo, MouseEvent, useCallback, useState } from 'react';
import { AppBarMenu } from '../menus';

interface ISearchAppBarProps {
  title: string;
}
export const SearchAppBar: FC<ISearchAppBarProps> = memo(({ title }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const onOpen = useCallback(
    (event: MouseEvent<HTMLElement>) => {
      setAnchorEl(event.currentTarget);
    },
    [setAnchorEl]
  );

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            sx={{ mr: 2 }}
            onClick={onOpen}
            aria-label="open menu"
          >
            <MenuIcon />
          </IconButton>
          <Typography
            variant="h6"
            noWrap
            component="div"
            sx={{ flexGrow: 1, display: { xs: 'none', sm: 'block' } }}
          >
            {title}
          </Typography>
        </Toolbar>
      </AppBar>
      <AppBarMenu anchorEl={anchorEl} setAnchorEl={setAnchorEl} />
    </Box>
  );
});
