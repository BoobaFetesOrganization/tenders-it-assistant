//import { Search } from '@mui/icons-material';
import MenuIcon from '@mui/icons-material/Menu';
import { Input } from '@mui/material';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import { alpha, styled } from '@mui/material/styles';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { FC, memo, useCallback, useState } from 'react';

const Search = styled('div')(({ theme }) => ({
  position: 'relative',
  borderRadius: theme.shape.borderRadius,
  backgroundColor: alpha(theme.palette.common.white, 0.15),
  '&:hover': {
    backgroundColor: alpha(theme.palette.common.white, 0.25),
  },
  marginLeft: 0,
  width: '100%',
  [theme.breakpoints.up('sm')]: {
    marginLeft: theme.spacing(1),
    width: 'auto',
  },
}));

const SearchIconWrapper = styled('div')(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: '100%',
  position: 'absolute',
  pointerEvents: 'none',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
}));

const DeleteFilterIconWrapper = styled('div')(({ theme }) => ({
  padding: theme.spacing(0, 1, 0, 1),
  height: '100%',
  position: 'absolute',
  right: 0,
  cursor: 'pointer',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-end',
}));

const StyledInput = styled(Input)(({ theme }) => ({
  color: 'inherit',
  width: `100%`,
  '& .MuiInputBase-input': {
    padding: theme.spacing(1, 1, 1, 0),
    // vertical padding + font size from searchIcon
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create('width'),
    [theme.breakpoints.up('sm')]: {
      margin: theme.spacing(0, 3, 0, 0),
      width: '12ch',
      '&:focus': {
        width: '20ch',
      },
    },
  },
}));

interface ISearchAppBarProps {
  title: string;
}
export const SearchAppBar: FC<ISearchAppBarProps> = memo(({ title }) => {
  const [menuIsOpen, setMenuIsOpen] = useState(false);
  const toggleMenu = useCallback(
    () => setMenuIsOpen(!menuIsOpen),
    [menuIsOpen]
  );

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="open drawer"
            sx={{ mr: 2 }}
            onClick={toggleMenu}
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
          {/* <Search>
            <SearchIconWrapper>
              <SearchIcon />
            </SearchIconWrapper>
            <StyledInput
              placeholder="Search…"
              inputProps={{ 'aria-label': 'search' }}
              endAdornment={
                <DeleteFilterIconWrapper onClick={() => setFilter(null)}>
                  <CloseIcon />
                </DeleteFilterIconWrapper>
              }
              onChange={(e) => setFilter(e.target.value.toLowerCase())}
            />
          </Search> */}
        </Toolbar>
      </AppBar>
    </Box>
  );
});
