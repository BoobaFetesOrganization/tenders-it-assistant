import { RouteObject } from 'react-router';
import { DashBoardWrapper } from './DashBoardWrapper';
import { projectRoutes } from './project';

export const routes: RouteObject[] = [
  {
    path: '/',
    children: [
      {
        index: true,
        element: <DashBoardWrapper />,
      },
      ...projectRoutes,
    ],
  },
];
