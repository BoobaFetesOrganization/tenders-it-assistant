import { GetRequestLinkType } from './GetLinkType';

const origin = window.location.origin;

export const getCorsLink: GetRequestLinkType = () => {
  return (operation, forward) => {
    operation.setContext(({ headers = {} }) => ({
      headers: {
        ...headers,
        'Access-Control-Allow-Origin': origin,
        'Access-Control-Allow-Headers': '*',
        'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
      },
    }));

    return forward(operation);
  };
};
