import { ApolloClient, DefaultOptions, InMemoryCache } from '@apollo/client';
import { IInfraSettings } from '../settings';
import { getLink } from './link';

const defaultOptions: DefaultOptions = {
  watchQuery: {
    fetchPolicy: 'cache-and-network',
  },
};

export function getClient(settings: IInfraSettings) {
  return new ApolloClient({
    defaultOptions,
    link: getLink(settings),
    cache: new InMemoryCache(),
  });
}
