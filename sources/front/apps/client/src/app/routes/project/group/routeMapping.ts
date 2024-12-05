import { NavLinkProps } from 'react-router';
import { projectRouteMapping } from '..';
import { IUserStoryGroupParams } from './IUserStoryGroupParams';

type UrlParameter = Omit<IUserStoryGroupParams, 'id'> &
  Partial<Pick<IUserStoryGroupParams, 'id'>>;

const segment = 'group';
const editorSegment = 'editor';
export const routeMapping = {
  segment,
  editorSegment,
  url({ projectId, id }: UrlParameter, action?: 'editor'): NavLinkProps {
    const parentMapping = projectRouteMapping.url({ id: projectId });
    const base = parentMapping.to + `/${segment}`;

    switch (action) {
      case 'editor':
        return {
          to: `${base}/${editorSegment}`,
        };
      default:
        return {
          to: !id ? base : `${base}/${id}`,
        };
    }
  },
};
