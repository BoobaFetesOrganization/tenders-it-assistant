import { ApolloClient, MutationResult } from '@apollo/client';

export function newMutationResult<T>(
  client: ApolloClient<object>
): MutationResult<T> {
  return {
    loading: false,
    called: false,
    data: undefined,
    error: undefined,
    client,
    reset: () => {
      throw new Error('Method not implemented.');
    },
  };
}
