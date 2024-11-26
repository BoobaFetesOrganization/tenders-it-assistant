import { IDocumentBaseDto, IProjectBaseDto, newPage } from '@aogenai/domain';
import { useDocuments } from '@aogenai/infra';
import CreateIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import {
  Button,
  Grid2,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Pagination,
  styled,
} from '@mui/material';
import { FC, memo, useCallback } from 'react';

interface IDocumentCollectionProps {
  projectId: IProjectBaseDto['id'];
  onCreate?: () => void;
  onDelete?: (item: IDocumentBaseDto) => void;
}

const maxItemPerPage = 10;
export const DocumentCollection: FC<IDocumentCollectionProps> = memo(
  ({ projectId, onCreate, onDelete }) => {
    const { data: { documents } = { documents: newPage() }, refetch } =
      useDocuments({
        variables: {
          projectId,
          offset: 0,
          limit: maxItemPerPage,
        },
      });
    // const [createDocument] = useCreateDocument();
    // const [deleteDocument] = useDeleteDocument();

    const onPageChange = useCallback(
      (event: React.ChangeEvent<unknown>, page: number) => {
        refetch({
          projectId,
          offset: (page - 1) * maxItemPerPage,
          limit: maxItemPerPage,
        });
      },
      [projectId, refetch]
    );

    return (
      <StyledRoot container className="collection-document">
        <StyledPagination>
          <CreateButtonItem>
            <Button id={`document-create`} startIcon={<CreateIcon />}>
              Create
            </Button>
          </CreateButtonItem>
          <Grid2 flexGrow={0}>
            <Pagination
              count={Math.ceil((documents.page.count ?? 0) / maxItemPerPage)}
              siblingCount={2}
              variant="outlined"
              color="primary"
              size="small"
              disabled={documents.data.length === 0}
              onChange={onPageChange}
              showFirstButton
              showLastButton
            />
          </Grid2>
        </StyledPagination>
        <StyledContent>
          <List>
            {documents.data.map((document: IDocumentBaseDto) => (
              <ListItem
                secondaryAction={
                  <IconButton
                    edge="end"
                    aria-label="delete"
                    onClick={() => onDelete?.(document)}
                  >
                    <DeleteIcon />
                  </IconButton>
                }
              >
                <ListItemText primary={document.name} />
              </ListItem>
            ))}
          </List>
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

const CreateButtonItem = styled(Grid2)(({ theme }) => ({
  margin: theme.spacing(0, 2, 0, 0),
}));
