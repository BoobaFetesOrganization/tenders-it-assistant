import { Menu, MenuItem, styled } from '@mui/material';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';

interface IAppBarMenuProps {
  anchorEl: HTMLElement | null;
  setAnchorEl(elemnt: HTMLElement | null): void;
}

export const AppBarMenu: FC<IAppBarMenuProps> = memo(
  ({ setAnchorEl, anchorEl }) => {
    const navigate = useNavigate();

    const onClose = useCallback(() => {
      setAnchorEl(null);
    }, [setAnchorEl]);

    const onNavigate = useCallback(
      (name: 'dashboard' | 'project' | 'document' | 'userStory') => () => {
        if (name === 'dashboard') navigate('/');
        else navigate(`/${name}`);
        onClose();
      },
      [navigate, onClose]
    );

    const onListKeyDown = useCallback(
      (event: React.KeyboardEvent) => {
        if (event.key === 'Tab') {
          event.preventDefault();
          onClose();
        } else if (event.key === 'Escape') {
          onClose();
        }
      },
      [onClose]
    );

    return (
      <StyledMenu
        id="appbar-menu"
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={onClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        onKeyDown={onListKeyDown}
      >
        <MenuItem onClick={onNavigate('dashboard')}>Dashboard</MenuItem>
        <MenuItem onClick={onNavigate('project')}>Projects</MenuItem>
        <MenuItem onClick={onNavigate('document')}>Documents</MenuItem>
        <MenuItem onClick={onNavigate('userStory')}>User stories</MenuItem>
      </StyledMenu>
    );
  }
);

const StyledMenu = styled(Menu)(({ theme }) => ({
  padding: theme.spacing(0),
  fontWeight: 'bold',
  '& .MuiPaper-root': {
    top: '64px !important',
    left: '0px !important',
    minWidth: '25%',
    backgroundColor: theme.palette.primary.light,
    color: theme.palette.common.white,
    borderRadius: '0 0 4px 0',
    [theme.breakpoints.down('sm')]: {
      top: '56px !important',
      minWidth: '100%',
    },
    '& .MuiMenuItem-root:hover': {
      backgroundColor: theme.palette.common.white,
      color: theme.palette.primary.light,
    },
  },
}));
