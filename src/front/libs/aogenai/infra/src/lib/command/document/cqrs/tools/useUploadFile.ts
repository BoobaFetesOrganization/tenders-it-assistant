import {
  ApolloError,
  FetchResult,
  MutationFunctionOptions,
  MutationHookOptions,
  MutationResult,
  MutationTuple,
  useApolloClient,
} from '@apollo/client';
import { useCallback, useMemo, useState } from 'react';
import { newMutationResult } from './newMutationResult';

export function useUploadFile<TResponse, TRequest>(
  uploadCommand: (variables: TRequest) => Promise<TResponse>,
  options?: MutationHookOptions<TResponse, TRequest>
): MutationTuple<TResponse, TRequest> {
  const client = useApolloClient();

  const [result, setResult] = useState<MutationResult<TResponse>>(
    newMutationResult(client)
  );

  const mutate = useCallback(
    async (
      fnOptions?: MutationFunctionOptions<TResponse, TRequest>
    ): Promise<FetchResult<TResponse>> => {
      const mutationResult = newMutationResult<TResponse>(client);
      const fetchResult: FetchResult<TResponse> = {};
      const variables = options?.variables || fnOptions?.variables;
      try {
        if (!variables) {
          throw new Error('Variables are required');
        }

        setResult({ ...mutationResult, loading: true, called: true });
        mutationResult.data = fetchResult.data = await uploadCommand(variables);
        mutationResult.error = undefined;
        options?.onCompleted?.(mutationResult.data, { client });
      } catch (e) {
        mutationResult.data = undefined;
        mutationResult.error = e as ApolloError;
        fetchResult.errors = [e as Error];
        options?.onError?.(e as ApolloError, { client });
      }

      setResult({ ...mutationResult, loading: false });
      return result;
    },
    [client, options, result, uploadCommand]
  );

  return useMemo(() => [mutate, result], [mutate, result]);
}
