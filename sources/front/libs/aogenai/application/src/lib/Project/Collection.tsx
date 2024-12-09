import { IProjectBaseDto, newPage } from '@aogenai/domain';
import { useProjects } from '@aogenai/infra';
import CreateIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import SendIcon from '@mui/icons-material/Send';
import {
  Box,
  Button,
  Grid2,
  IconButton,
  Pagination,
  Paper,
  Popover,
  styled,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import { Loading } from '../common';
import { ProjectCreate } from './Create';

interface IProjectCollectionProps {
  onCreated?: (item: IProjectBaseDto) => void;
  onSelect?: (item: IProjectBaseDto) => void;
  onDelete?: (item: IProjectBaseDto) => void;
}

const maxItemPerPage = 10;
export const ProjectCollection: FC<IProjectCollectionProps> = memo(
  ({ onCreated, onDelete, onSelect }) => {
    const [variables, setVariables] = useState({
      offset: 0,
      limit: maxItemPerPage,
    });
    const { data: { projects } = { projects: newPage() }, loading } =
      useProjects({
        variables,
      });

    const onPageChange = useCallback(
      (event: React.ChangeEvent<unknown>, page: number) => {
        setVariables({ ...variables, offset: (page - 1) * variables.limit });
      },
      [variables]
    );

    const [createAnchor, setCreateAnchor] = useState<null | HTMLElement>(null);
    const onCreateProjectClick = useCallback(
      (event: React.MouseEvent<HTMLElement>) => {
        setCreateAnchor(event.currentTarget);
      },
      [setCreateAnchor]
    );
    const onCreateProjectClose = useCallback(() => {
      setCreateAnchor(null);
    }, [setCreateAnchor]);

    return loading ? (
      <Loading />
    ) : (
      <StyledRoot container className="collection-project">
        <StyledPagination>
          {onCreated && (
            <CreateButtonItem>
              <IconButton color="primary" onClick={onCreateProjectClick}>
                <CreateIcon />
              </IconButton>
              <Popover
                open={Boolean(createAnchor)}
                anchorEl={createAnchor}
                onClose={onCreateProjectClose}
                anchorOrigin={{
                  vertical: 'bottom',
                  horizontal: 'left',
                }}
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'left',
                }}
              >
                <ProjectCreate onCreated={onCreated} />
              </Popover>
            </CreateButtonItem>
          )}
          <Grid2 flexGrow={0}>
            <Pagination
              count={Math.ceil((projects.page.count ?? 0) / maxItemPerPage)}
              siblingCount={2}
              variant="outlined"
              color="primary"
              size="small"
              disabled={projects.data.length === 0}
              onChange={onPageChange}
              showFirstButton
              showLastButton
            />
          </Grid2>
        </StyledPagination>
        <StyledContent>
          <TableContainer
            component={Paper}
            sx={{ overflow: 'auto', height: '100%' }}
          >
            <Table stickyHeader>
              <TableHead>
                <TableRow>
                  <TableCell>
                    <Typography fontWeight="bold">ID</Typography>
                  </TableCell>
                  <TableCell sx={{ width: '100%' }}>
                    <Typography fontWeight="bold">Name</Typography>
                  </TableCell>
                  {(onSelect || onDelete) && (
                    <TableCell>
                      <Typography fontWeight="bold">Actions</Typography>
                    </TableCell>
                  )}
                </TableRow>
              </TableHead>
              <TableBody>
                {projects.data.map((project) => (
                  <TableRow key={project.id}>
                    <StyledTableCell>{project.id}</StyledTableCell>
                    <StyledTableCell
                      sx={{
                        width: '100%',
                        '&:hover': {
                          textDecoration: onSelect && 'underline',
                        },
                      }}
                      onClick={() => onSelect?.(project)}
                    >
                      <Typography fontWeight="bold">{project.name}</Typography>
                    </StyledTableCell>
                    {(onSelect || onDelete) && (
                      <StyledTableCell>
                        <Box display="flex" justifyContent="flex-end">
                          {onDelete && (
                            <StyledItemButton
                              id={`project-delete-${project.id}`}
                              onClick={() => onDelete(project)}
                              color="error"
                              endIcon={<DeleteIcon />}
                            />
                          )}
                          {onSelect && (
                            <StyledItemButton
                              id={`project-navigate-${project.id}`}
                              onClick={() => onSelect(project)}
                              endIcon={<SendIcon />}
                            />
                          )}
                        </Box>
                      </StyledTableCell>
                    )}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </StyledContent>
      </StyledRoot>
    );
  }
);

const StyledRoot = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  flexDirection: 'column',
}));

const StyledPagination = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  padding: theme.spacing(0, 3, 0, 0),
  margin: theme.spacing(2, 0, 0, 0),
}));

const StyledContent = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  overflow: 'hidden',
  '& table > tbody> tr>td': { cursor: 'context-menu' },
}));

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  padding: theme.spacing(1),
}));

const CreateButtonItem = styled(Grid2)(({ theme }) => ({
  margin: theme.spacing(0, 2, 0, 0),
}));

const StyledItemButton = styled(Button)(({ theme }) => ({
  padding: theme.spacing(0, 2),
}));
