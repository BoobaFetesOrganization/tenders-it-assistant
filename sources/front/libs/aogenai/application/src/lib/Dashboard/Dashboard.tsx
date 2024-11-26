import { useAllProjects } from '@aogenai/infra';
import CloseIcon from '@mui/icons-material/Close';
import { Box, Grid2, IconButton, Input, Typography } from '@mui/material';
import { DefaultizedPieValueType, PieItemIdentifier } from '@mui/x-charts';
import { PieChart } from '@mui/x-charts/PieChart';
import { FC, memo, MouseEvent, useCallback, useState } from 'react';
import { useNavigate } from 'react-router';

export const DashBoard: FC = memo(() => {
  const navigate = useNavigate();
  const projects = useAllProjects();
  const [projectName, setProjectName] = useState('');

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
    (
      event: MouseEvent,
      pieItemIdentifier: PieItemIdentifier,
      item: DefaultizedPieValueType
    ) => {
      navigate(`/project/${item.id}`);
    },
    [navigate]
  );

  return (
    <Grid2 container flex={1} flexDirection="column">
      <Grid2>
        <Typography variant="h3">Dashboard</Typography>
      </Grid2>
      <Grid2 container flex={0}>
        <Box>
          <Typography variant="body1">Projects</Typography>
          <Input
            placeholder="filter by name"
            value={projectName}
            onChange={onProjectNameChange}
            endAdornment={
              <IconButton onClick={onProjectNameClear}>
                <CloseIcon />
              </IconButton>
            }
          />
          <PieChart
            onItemClick={onProjectClick}
            series={[
              {
                data: projects
                  .reverse()
                  .filter(
                    (i, index) =>
                      index < 18 && i.name.toLowerCase().includes(projectName)
                  )
                  .map((i) => ({
                    id: i.id,
                    value: 1,
                    label: i.name,
                  })),
                innerRadius: 30,
                outerRadius: 100,
                paddingAngle: 0,
                cornerRadius: 5,
                cx: 150,
                cy: 150,
              },
            ]}
            width={550}
            height={300}
          />
        </Box>
      </Grid2>
      <Grid2></Grid2>
    </Grid2>
  );
});
