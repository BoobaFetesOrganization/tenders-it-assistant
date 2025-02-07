import { NavLinkProps } from 'react-router';
import { IProjectParams } from './IProjectParams';

type UrlParameter = Omit<IProjectParams, 'id'> &
  Partial<Pick<IProjectParams, 'id'>>;

const segment = 'project';

export const routeMapping = {
  segment,
  url({ id }: UrlParameter = { id: undefined }): NavLinkProps {
    const base = `/${segment}`;
    return {
      to: !id ? base : `${base}/${id}`,
    };
  },
  urlToEditor({ id }: UrlParameter = { id: undefined }): NavLinkProps {
    const base = this.url({ id });
    return {
      ...base,
      to: `${base.to}/group-editor`,
    };
  },
};
