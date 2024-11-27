import { IDocumentBaseDto, IProjectBaseDto, newPage } from '@aogenai/domain';
import {
  useCreateDocument,
  useDeleteDocument,
  useDocuments,
  useUpdateDocument,
} from '@aogenai/infra';
import DeleteIcon from '@mui/icons-material/Delete';
import {
  Grid2,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Pagination,
  styled,
} from '@mui/material';
import { FC, memo, useCallback } from 'react';
import { DropZone } from './DropZone';

interface IDocumentCollectionProps {
  projectId: IProjectBaseDto['id'];
  onSelected?: (item: IDocumentBaseDto) => void;
  onCreated?: (item: IDocumentBaseDto) => void;
  onUpdated?: (item: IDocumentBaseDto) => void;
  onDeleted?: (item: IDocumentBaseDto) => void;
}

const maxItemPerPage = 10;
export const DocumentCollection: FC<IDocumentCollectionProps> = memo(
  ({ projectId, onCreated, onDeleted, onSelected, onUpdated }) => {
    const { data: { documents } = { documents: newPage() }, refetch } =
      useDocuments({
        variables: {
          projectId,
          offset: 0,
          limit: maxItemPerPage,
        },
      });
    const [createDocument] = useCreateDocument({
      onCompleted: ({ document }) => onCreated?.(document),
    });
    const [updateDocument] = useUpdateDocument({
      onCompleted: ({ document }) => onUpdated?.(document),
    });
    const [deleteDocument] = useDeleteDocument({
      onCompleted: ({ document }) => onDeleted?.(document),
    });

    const onDelete = useCallback(
      (document: IDocumentBaseDto) => () => {
        deleteDocument({ variables: { projectId, id: document.id } });
      },
      [deleteDocument, projectId]
    );

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

    const onDrop = useCallback(
      (acceptedFiles: File[]) => {
        if (acceptedFiles.length > 0) {
          acceptedFiles.forEach((file) => {
            if (documents.data.find((doc) => doc.name === file.name)) {
              updateDocument({ variables: { projectId, input: { file } } });
            } else {
              createDocument({ variables: { projectId, input: { file } } });
            }
          });
        }
      },
      [createDocument, documents.data, projectId, updateDocument]
    );

    return (
      <StyledRoot container className="collection-document">
        <StyledPagination>
          <DropZone onDrop={onDrop} />
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
                    onClick={onDelete(document)}
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
