import { IProjectBaseDto, IProjectDto } from '@aogenai/domain';
import { useProject } from '@aogenai/infra';
import { useCallback, useEffect, useState } from 'react';

interface IUseFetchCurrentDataParams {
  projects: IProjectBaseDto[];
}
export const useFetchCurrentData = ({
  projects,
}: IUseFetchCurrentDataParams) => {
  const { fetchMore: fetchProject } = useProject();
  const [result, setResults] = useState<IProjectDto[]>();

  const fetchData = useCallback(async () => {
    const lastTenProjects = projects.slice(-10); // Prendre les 10 derniers projets

    // fetch a collection of group bases for the last 10 projects
    const settledItems = await Promise.allSettled(
      lastTenProjects.map(({ id }) => fetchProject({ variables: { id } }))
    );
    const items = settledItems
      .filter((i) => i.status === 'fulfilled')
      .map((i) => i.value.data.project);

    setResults(items);
  }, [fetchProject, projects]);

  useEffect(() => {
    if (!result && projects.length > 0) fetchData();
  }, [fetchData, projects, result]);

  return result || [];
};
