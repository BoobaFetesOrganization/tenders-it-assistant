import { IProjectDto } from '@aogenai/domain';
import SendIcon from '@mui/icons-material/Send';
import { Box, Button, styled } from '@mui/material';
import { FC, memo, useCallback } from 'react';
import { DataColumn, DataTable } from '../common';

interface IProjectsToEstimateProps {
  data: IProjectDto[];
  onSelect?(item: IProjectDto): void;
  onEstimate?(item: IProjectDto): void;
}

export const ProjectsToEstimate: FC<IProjectsToEstimateProps> = memo(
  ({ data, onSelect, onEstimate }) => {
    const Actions: FC<IProjectDto> = useCallback(
      (project) =>
        !!onEstimate && <ProjectActions {...project} onEstimate={onEstimate} />,
      [onEstimate]
    );
    return (
      <DataTable<IProjectDto> data={data} actions={Actions}>
        <DataColumn<IProjectDto> title="ID" source="id" />
        <DataColumn<IProjectDto>
          title="Name"
          source="name"
          fill
          onSelect={onSelect}
        >
          {/* {!project.documents.length && (
              <Typography color="error">
                (at least one document is required)
              </Typography>
            )} */}
        </DataColumn>
      </DataTable>
    );
  }
);

const ProjectActions: FC<
  IProjectDto & {
    onEstimate(item: IProjectDto): void;
  }
> = memo(({ onEstimate, ...project }) => (
  <Box display="flex" justifyContent="flex-end">
    <StyledItemButton
      id={`project-estimate-${project.id}`}
      onClick={() => onEstimate(project)}
      endIcon={<SendIcon />}
      disabled={!project.documents.length}
    />
  </Box>
));

const StyledItemButton = styled(Button)(({ theme }) => ({
  padding: theme.spacing(0, 2),
}));
