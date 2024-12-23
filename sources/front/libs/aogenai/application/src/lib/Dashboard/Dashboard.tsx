import { IProjectBaseDto, IProjectDto } from '@aogenai/domain';
import CloseIcon from '@mui/icons-material/Close';
import {
  Box,
  Grid2,
  IconButton,
  Input,
  Paper,
  styled,
  Typography,
} from '@mui/material';
import { DefaultizedPieValueType, PieItemIdentifier } from '@mui/x-charts';
import { PieChart } from '@mui/x-charts/PieChart';
import { FC, memo, MouseEvent, useCallback, useState } from 'react';
import { ProjectCollection } from '../Project';
import { ProjectsToEstimate } from './ProjectToEstimate';
import { useEstimatedProjects } from './useEstimatedProjects';

interface IDashBoardProps {
  onProjectSelected?(id: number): void;
  onProjectCreated?: (item: IProjectBaseDto) => void;
  onProjectDeleted?: (item: IProjectBaseDto) => void;
  onProjectEstimate?: (item: IProjectDto) => void;
}
export const DashBoard: FC<IDashBoardProps> = memo(
  ({
    onProjectSelected,
    onProjectCreated,
    onProjectDeleted,
    onProjectEstimate,
  }) => {
    const [projectName, setProjectName] = useState('');

    const { estimation, pieData } = useEstimatedProjects();

    const onProjectNameChange = useCallback(
      (event: React.ChangeEvent<HTMLInputElement>) => {
        setProjectName(event.target.value);
      },
      []
    );
    const onProjectNameClear = useCallback(() => {
      setProjectName('');
    }, []);

    const onProjectClick = useCallback(
      (item: IProjectBaseDto) => {
        onProjectSelected?.(item.id);
      },
      [onProjectSelected]
    );
    const onPieChartProjectClick = useCallback(
      (
        event: MouseEvent,
        pieItemIdentifier: PieItemIdentifier,
        item: DefaultizedPieValueType
      ) => {
        onProjectSelected?.(+item.id);
      },
      [onProjectSelected]
    );

    return (
      <Grid2 container flex={1} flexDirection="column">
        <Grid2>
          <Typography variant="h3">Dashboard</Typography>
        </Grid2>
        <Grid2 container spacing={1}>
          <Grid2
            container
            flex={1}
            spacing={2}
            size={{ xs: 12, sm: 12, md: 12, lg: 6 }}
          >
            <CustomPaper>
              <Box display="flex" flexDirection="column" flexGrow={1}>
                <Typography variant="h5">
                  <u>{'Projects'.toUpperCase()}</u>
                </Typography>
                <ProjectCollection
                  onSelect={onProjectClick}
                  onCreated={onProjectCreated}
                  onDelete={onProjectDeleted}
                />
              </Box>
            </CustomPaper>
          </Grid2>
          {!!estimation.remaining.length && (
            <Grid2
              container
              flex={1}
              spacing={2}
              size={{ xs: 12, sm: 12, md: 12, lg: 6 }}
            >
              <CustomPaper>
                <Box display="flex" flexDirection="column" flexGrow={1}>
                  <Typography variant="h5">
                    <u>{'Projects to estimate'.toUpperCase()}</u>
                  </Typography>
                  <ProjectsToEstimate
                    data={estimation.remaining}
                    onSelect={onProjectSelected}
                    onEstimate={onProjectEstimate}
                  />
                </Box>
              </CustomPaper>
            </Grid2>
          )}
          <Grid2
            container
            flex={1}
            spacing={2}
            size={{ xs: 12, sm: 12, md: 12, lg: 6 }}
          >
            <CustomPaper>
              <Box display="flex" flexDirection="column" flexGrow={1}>
                <Typography variant="h5">
                  <u>{'Project graph'.toUpperCase()}</u>
                </Typography>
                <Input
                  placeholder="filter by name"
                  value={projectName}
                  onChange={onProjectNameChange}
                  endAdornment={
                    <IconButton onClick={onProjectNameClear}>
                      <CloseIcon />
                    </IconButton>
                  }
                  fullWidth
                />
                <PieChart
                  onItemClick={onPieChartProjectClick}
                  series={[pieData]}
                  width={550}
                  height={300}
                />
              </Box>
            </CustomPaper>
          </Grid2>
        </Grid2>
      </Grid2>
    );
  }
);

const CustomPaper = styled(Paper)(({ theme }) => ({
  padding: theme.spacing(2),
  margin: theme.spacing(2),
  backgroundColor: theme.palette.grey[100],
  display: 'inherit',
  flexDirection: 'inherit',
  flex: '1 1 auto',
  overflow: 'auto',
}));
