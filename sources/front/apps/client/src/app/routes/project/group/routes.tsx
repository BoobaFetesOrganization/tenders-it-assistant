import { FC } from 'react';
import { RouteObject, useParams } from 'react-router';
import { routeMapping } from './routeMapping';

const UserStoryGroupCollectionWrapper: FC = () => {
  const params = useParams();
  return (
    <div>
      <h5>UserStoryGroupCollectionWrapper</h5>
      <div>
        <p>params</p>
        <p>{JSON.stringify(params)}</p>
      </div>
      <div>
        <p>not implemented yet</p>
      </div>
    </div>
  );
};
const UserStoryGroupItemWrapper: FC = () => {
  const params = useParams();
  return (
    <div>
      <h5>UserStoryGroupItemWrapper</h5>
      <div>
        <p>params</p>
        <p>{JSON.stringify(params)}</p>
      </div>
      <div>
        <p>not implemented yet</p>
      </div>
    </div>
  );
};

export const userstoryGoupRoutes: RouteObject[] = [
  {
    path: routeMapping.segment,
    children: [
      {
        index: true,
        element: <UserStoryGroupCollectionWrapper />,
      },
      {
        path: ':groupId',
        element: <UserStoryGroupItemWrapper />,
      },
    ],
  },
];
