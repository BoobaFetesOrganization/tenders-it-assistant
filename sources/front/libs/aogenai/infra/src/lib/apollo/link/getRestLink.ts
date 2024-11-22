import { RestLink } from 'apollo-link-rest';
import { IInfraSettings } from '../../settings';

export function getRestLink({ api: { url } }: IInfraSettings) {
  return new RestLink({ uri: url });
}
