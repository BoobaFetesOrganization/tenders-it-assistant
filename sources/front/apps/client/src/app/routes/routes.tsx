import { DashBoard } from '@aogenai/application';
import { RouteObject } from 'react-router';
import { projectRoutes } from './project';

export const routes: RouteObject[] = [
  {
    path: '/',
    children: [
      {
        index: true,
        element: <DashBoard />,
      },
      ...projectRoutes,
    ],
  },
];
