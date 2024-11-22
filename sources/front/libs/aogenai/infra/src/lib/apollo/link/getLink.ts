import { from } from '@apollo/client';
import { IInfraSettings } from '../../settings';
import { getRestLink } from './getRestLink';

export const getLink = (settings: IInfraSettings) =>
  from([getRestLink(settings)]);
