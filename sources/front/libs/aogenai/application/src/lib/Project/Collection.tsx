import { IProjectBaseDto, newPage } from '@aogenai/domain';
import { useProjects } from '@aogenai/infra';
import CreateIcon from '@mui/icons-material/Add';
import {
  Button,
  Grid2,
  Pagination,
  Paper,
  styled,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';
import { FC, memo, ReactNode, useCallback, useState } from 'react';

interface IProjectCollectionProps {
  onCreate?: () => void;
  onActions?: (item: IProjectBaseDto) => ReactNode;
}

const maxItemPerPage = 10;
export const Collection: FC<IProjectCollectionProps> = memo(
  ({ onActions, onCreate }) => {
    const [variables, setVariables] = useState({
      offset: 0,
      limit: maxItemPerPage,
    });
    const { data: { projects } = { projects: newPage() } } = useProjects({
      variables,
    });

    const onPageChange = useCallback(
      (event: React.ChangeEvent<unknown>, page: number) => {
        setVariables({ ...variables, offset: (page - 1) * variables.limit });
      },
      [variables]
    );

    return (
      <StyledRoot container>
        <StyledTitle>
          <Typography variant="h3">Projects</Typography>
        </StyledTitle>
        <StyledPagination>
          {onCreate && (
            <CreateButtonItem>
              <Button
                id={`project-create`}
                onClick={onCreate}
                startIcon={<CreateIcon />}
              >
                Create
              </Button>
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
                  {onActions && (
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
                    <StyledTableCell sx={{ width: '100%' }}>
                      <Typography fontWeight="bold">{project.name}</Typography>
                    </StyledTableCell>
                    {onActions && (
                      <StyledTableCell>{onActions(project)}</StyledTableCell>
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

const StyledTitle = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 0,
  textDecoration: 'underline',
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

const CreateButtonItem = styled(Grid2)(({ theme }) => ({
  margin: theme.spacing(0, 2, 0, 0),
}));

const StyledContent = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  overflow: 'hidden',
}));

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  padding: theme.spacing(1),
}));
