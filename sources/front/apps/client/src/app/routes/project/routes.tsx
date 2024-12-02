import { RouteObject } from 'react-router';
import { ProjectCollectionWrapper } from './ProjectCollectionWrapper';
import { ProjectItemWrapper } from './ProjectItemWrapper';
import { routeMapping } from './routeMapping';

export const projectRoutes: RouteObject[] = [
  {
    path: routeMapping.root,
    element: <ProjectCollectionWrapper />,
  },
  {
    path: routeMapping.item,
    element: <ProjectItemWrapper />,
  },
];
