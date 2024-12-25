import { IProjectBaseDto, newPage } from '@aogenai/domain';
import { useProjects } from '@aogenai/infra';
import CreateIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import SendIcon from '@mui/icons-material/Send';
import { Box, Button, Grid2, IconButton, Popover, styled } from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import { DataColumn, DataPagination, DataTable, Loading } from '../common';
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

    const Actions = useCallback<FC<IProjectBaseDto>>(
      (item) => (
        <ProjectActions {...item} onDelete={onDelete} onSelect={onSelect} />
      ),
      [onDelete, onSelect]
    );

    return loading ? (
      <Loading />
    ) : (
      <DataTable<IProjectBaseDto>
        data={projects.data}
        actions={Actions}
        sticky
        className="collection-project"
      >
        <DataPagination
          count={projects.page.count}
          maxItemPerPage={maxItemPerPage}
          onChange={onPageChange}
        >
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
        </DataPagination>
        <DataColumn<IProjectBaseDto> title="ID" source="id" />
        <DataColumn<IProjectBaseDto>
          title="Name"
          source="name"
          fill
          onSelect={onSelect}
        />
      </DataTable>
    );
  }
);

const ProjectActions: FC<
  IProjectBaseDto & {
    onSelect?(item: IProjectBaseDto): void;
    onDelete?(item: IProjectBaseDto): void;
  }
> = ({ onSelect, onDelete, ...project }) => {
  return (
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
  );
};

const CreateButtonItem = styled(Grid2)(({ theme }) => ({
  margin: theme.spacing(0, 2, 0, 0),
}));

const StyledItemButton = styled(Button)(({ theme }) => ({
  padding: theme.spacing(0, 2),
}));
