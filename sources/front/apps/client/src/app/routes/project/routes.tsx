import { RouteObject } from 'react-router';
import { ProjectCollection } from './ProjectCollection';
import { ProjectEdit } from './ProjectEdit';

export const projectRoutes: RouteObject[] = [
  {
    path: 'project/*',
    element: <ProjectCollection />,
  },
  {
    path: 'project/:id',
    element: <ProjectEdit />,
  },
];
