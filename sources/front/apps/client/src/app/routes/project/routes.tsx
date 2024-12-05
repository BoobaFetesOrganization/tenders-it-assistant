import { RouteObject } from 'react-router';
import { userstoryGoupRoutes } from './group';
import { ProjectCollectionWrapper } from './ProjectCollectionWrapper';
import { ProjectItemWrapper } from './ProjectItemWrapper';
import { ProjectStoriesEditorWrapper } from './ProjectStoriesEditorWrapper';
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
            path: 'group-editor',
            element: <ProjectStoriesEditorWrapper />,
          },
          ...userstoryGoupRoutes,
        ],
      },
    ],
  },
];
