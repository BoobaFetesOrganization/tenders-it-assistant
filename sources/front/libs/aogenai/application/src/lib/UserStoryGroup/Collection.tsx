import { IUserStoryGroupBaseDto, newPage } from '@aogenai/domain';
import { useUserStoryGroups } from '@aogenai/infra';
import CreateIcon from '@mui/icons-material/Add';
import CloseIcon from '@mui/icons-material/Close';
import SendIcon from '@mui/icons-material/Send';
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
import { FC, memo, useCallback, useState } from 'react';
import { Loading } from '../common';

interface IUserStoryGroupCollectionProps {
  projectId: number;
  onCreate?: () => void;
  onSelect?: (item: IUserStoryGroupBaseDto) => void;
  onDelete?: (item: IUserStoryGroupBaseDto) => void;
}

const maxItemPerPage = 10;
export const UserStoryGroupCollection: FC<IUserStoryGroupCollectionProps> =
  memo(({ projectId, onCreate, onDelete, onSelect }) => {
    const [variables, setVariables] = useState({
      offset: 0,
      limit: maxItemPerPage,
    });
    const { data: { groups } = { groups: newPage() }, loading } =
      useUserStoryGroups({
        variables: { ...variables, projectId },
      });

    const onPageChange = useCallback(
      (event: React.ChangeEvent<unknown>, page: number) => {
        setVariables({ ...variables, offset: (page - 1) * variables.limit });
      },
      [variables]
    );

    return loading ? (
      <Loading />
    ) : (
      <StyledRoot container className="collection-project">
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
              count={Math.ceil((groups.page.count ?? 0) / maxItemPerPage)}
              siblingCount={2}
              variant="outlined"
              color="primary"
              size="small"
              disabled={groups.data.length === 0}
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
                  {onCreate && (
                    <TableCell>
                      <Typography fontWeight="bold">Actions</Typography>
                    </TableCell>
                  )}
                </TableRow>
              </TableHead>
              <TableBody>
                {groups.data.map((group, index) => (
                  <TableRow key={group.id}>
                    <StyledTableCell>{group.id}</StyledTableCell>
                    <StyledTableCell
                      sx={{
                        width: '100%',
                        '&:hover': {
                          textDecoration: onSelect && 'underline',
                        },
                      }}
                      onClick={() => onSelect?.(group)}
                    >
                      <Typography fontWeight="bold">Group {index}</Typography>
                    </StyledTableCell>
                    {(onSelect || onDelete) && (
                      <StyledTableCell>
                        {onDelete && (
                          <StyledItemButton
                            id={`project-delete-${group.id}`}
                            onClick={() => onDelete(group)}
                            endIcon={<CloseIcon />}
                          />
                        )}
                        {onSelect && (
                          <StyledItemButton
                            id={`project-navigate-${group.id}`}
                            onClick={() => onSelect(group)}
                            endIcon={<SendIcon />}
                          />
                        )}
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
  });

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
