import { from } from '@apollo/client';
import { IInfraSettings } from '../../settings';
import { getCorsLink } from './getCorsLink';
import { GetApolloLinkType } from './GetLinkType';
import { getRestLink } from './getRestLink';

export const getLink: GetApolloLinkType = (settings: IInfraSettings) =>
  from([getCorsLink(settings), getRestLink(settings)]);
