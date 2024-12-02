import { NavLinkProps } from 'react-router';
import { IUserStoryGroupParams } from './IUserStoryGroupParams';

type UrlParameter = Omit<IUserStoryGroupParams, 'id'> &
  Partial<Pick<IUserStoryGroupParams, 'id'>>;

export const routeMapping = {
  root: 'group/*',
  item: 'group/:id',
  url({ projectId, id }: UrlParameter): NavLinkProps {
    return {
      to:
        id === undefined
          ? `/project/${projectId}/group`
          : `/project/${projectId}/group/${id}`,
    };
  },
};
