import { NavLinkProps } from 'react-router';
import { projectRouteMapping } from '..';
import { IUserStoryGroupParams } from './IUserStoryGroupParams';

type UrlParameter = Omit<IUserStoryGroupParams, 'id'> &
  Partial<Pick<IUserStoryGroupParams, 'id'>>;

const segment = 'group';
export const routeMapping = {
  segment,
  url({ projectId, id }: UrlParameter): NavLinkProps {
    const parentMapping = projectRouteMapping.url({ id: projectId });
    const base = parentMapping.to + `/${segment}`;

    return {
      to: !id ? base : `${base}/${id}`,
    };
  },
};
