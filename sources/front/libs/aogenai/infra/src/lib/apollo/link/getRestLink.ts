import { RestLink } from 'apollo-link-rest';
import { IInfraSettings } from '../../settings';
import { GetApolloLinkType } from './GetLinkType';

export const getRestLink: GetApolloLinkType = ({
  api: { url },
}: IInfraSettings) => {
  return new RestLink({ uri: url });
};
