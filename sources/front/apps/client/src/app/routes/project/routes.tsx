import { RouteObject } from 'react-router';
import { userstoryGoupRoutes } from './group';
import { ProjectCollectionWrapper } from './ProjectCollectionWrapper';
import { ProjectItemWrapper } from './ProjectItemWrapper';
import { routeMapping } from './routeMapping';

export const projectRoutes: RouteObject[] = [
  {
    path: routeMapping.segment,
    children: [
      { index: true, element: <ProjectCollectionWrapper /> },
      {
        path: ':projectId',
        element: <ProjectItemWrapper />,
      },
      {
        path: ':projectId/*',
        children: [
          {
            path: 'toto',
            element: <div>toto</div>,
          },
          ...userstoryGoupRoutes,
        ],
      },
    ],
  },
];
