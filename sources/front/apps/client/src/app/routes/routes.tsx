import { DashBoard } from '@aogenai/application';
import { RouteObject } from 'react-router';
import { projectRoutes } from './project';

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <DashBoard />,
  },
  ...projectRoutes,
];
