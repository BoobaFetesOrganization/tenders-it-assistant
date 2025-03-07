import { PieChartProps } from '@mui/x-charts/PieChart';
import { IProjectDto, IUserStoryGroupDto } from '@tenders-it-assistant/domain';
import { useAllProjects } from '@tenders-it-assistant/infra';
import { useMemo } from 'react';
import { useFetchCurrentData } from './useFetchCurrentData';

type SeriesItemType = PieChartProps['series'][number];

const DATA_OPTIONS: Omit<SeriesItemType, 'data'> = {
  innerRadius: 30,
  outerRadius: 100,
  paddingAngle: 0,
  cornerRadius: 5,
  cx: 150,
  cy: 150,
};

export const useEstimatedProjects = () => {
  const projectBases = useAllProjects();

  function getValue(group: IUserStoryGroupDto | undefined) {
    return group?.userStories.reduce((acc, i) => acc + i.cost, 0) ?? 0;
  }
  const projects = useFetchCurrentData({
    projects: projectBases,
  });

  return useMemo(() => {
    const estimatedProjects: IProjectDto[] = [];
    const toBeEstimatedProjects: IProjectDto[] = [];
    const data: SeriesItemType['data'] = [];
    for (const project of projects) {
      if (!project.selectedGroup) {
        toBeEstimatedProjects.push(project);
      } else {
        estimatedProjects.push(project);
        data.push({
          id: project.id,
          value: getValue(project.selectedGroup),
          label: project.name,
        });
      }
    }
    return {
      projects,
      pieData: { ...DATA_OPTIONS, data },
      estimation: {
        done: estimatedProjects,
        remaining: toBeEstimatedProjects,
      },
    };
  }, [projects]);
};
