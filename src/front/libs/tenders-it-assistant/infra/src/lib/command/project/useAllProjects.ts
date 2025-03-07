import { IProjectBaseDto, newPage } from '@tenders-it-assistant/domain';
import { useEffect, useState } from 'react';
import { getInfraSettings } from '../../settings';
import { useProjects } from './useProjects';

export const useAllProjects = () => {
  const [projects, setProjects] = useState<IProjectBaseDto[]>([]);
  const { fetchMore } = useProjects({ skip: true });

  useEffect(() => {
    fetchAll(fetchMore, setProjects);
  }, [fetchMore]);

  return projects;
};

const fetchAll = async (
  fetchMore: ReturnType<typeof useProjects>['fetchMore'],
  setProjects: (projects: IProjectBaseDto[]) => void
) => {
  const maxLimit = getInfraSettings().api.maxLimit;
  const queue: Parameters<typeof fetchMore>[0]['variables'][] = [
    { offset: 0, limit: maxLimit },
  ];

  const results: IProjectBaseDto[] = [];

  while (queue.length > 0) {
    const { data: { projects } = { projects: newPage() } } = await fetchMore({
      variables: queue.shift(),
    });

    results.push(...projects.data);
    if (results.length < projects.page.count) {
      queue.push({ offset: results.length, limit: maxLimit });
    }
  }

  setProjects(results);
};
