import { RouteObject } from 'react-router';
import { UserStoryGroupCollectionWrapper } from './UserStoryGroupCollectionWrapper';
import { UserStoryGroupItemWrapper } from './UserStoryGroupItemWrapper';
import { routeMapping } from './routeMapping';

export const projectRoutes: RouteObject[] = [
  {
    path: routeMapping.root,
    element: <UserStoryGroupCollectionWrapper />,
  },
  {
    path: routeMapping.item,
    element: <UserStoryGroupItemWrapper />,
  },
];
