import { RouteObject } from 'react-router';
import { ProjectCollection, projectRoutes } from './project';

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <ProjectCollection />,
  },
  ...projectRoutes,
];
