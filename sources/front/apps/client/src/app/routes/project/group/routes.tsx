import { FC } from 'react';
import { RouteObject, useParams } from 'react-router';
import { routeMapping } from './routeMapping';
import { UserStoryGroupEditorWrapper } from './UserStoryGroupEditorWrapper';

const UserStoryGropuCollectionWrapper: FC = () => {
  const params = useParams();
  return (
    <div>
      <h5>UserStoryGropuCollectionWrapper</h5>
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
const UserStoryGropuItemWrapper: FC = () => {
  const params = useParams();
  return (
    <div>
      <h5>UserStoryGropuItemWrapper</h5>
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
        element: <UserStoryGropuCollectionWrapper />,
      },
      {
        path: routeMapping.editorSegment,
        element: <UserStoryGroupEditorWrapper />,
      },
      {
        path: ':groupId',
        element: <UserStoryGropuItemWrapper />,
      },
    ],
  },
];
