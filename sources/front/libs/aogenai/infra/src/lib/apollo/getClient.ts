import { ApolloClient, InMemoryCache } from '@apollo/client';
import { IInfraSettings } from '../settings';
import { getLink } from './link';

export function getClient(settings: IInfraSettings) {
  return new ApolloClient({
    link: getLink(settings),
    cache: new InMemoryCache(),
  });
}
