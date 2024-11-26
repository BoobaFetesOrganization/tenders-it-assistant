import { RouteObject } from 'react-router';
import { ProjectCollectionWrapper } from './ProjectCollectionWrapper';
import { ProjectItemWrapper } from './ProjectItemWrapper';

export const projectRoutes: RouteObject[] = [
  {
    path: 'project/*',
    element: <ProjectCollectionWrapper />,
  },
  {
    path: 'project/:id',
    element: <ProjectItemWrapper />,
  },
];
