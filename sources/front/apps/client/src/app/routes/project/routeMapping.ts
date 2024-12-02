import { NavLinkProps } from 'react-router';
import { IProjectParams } from './IProjectParams';

type UrlParameter = Omit<IProjectParams, 'id'> &
  Partial<Pick<IProjectParams, 'id'>>;

export const routeMapping = {
  root: 'project/*',
  item: 'project/:id',
  url({ id }: UrlParameter = { id: undefined }): NavLinkProps {
    return {
      to: id === undefined ? `/project` : `/project/${id}`,
    };
  },
};
