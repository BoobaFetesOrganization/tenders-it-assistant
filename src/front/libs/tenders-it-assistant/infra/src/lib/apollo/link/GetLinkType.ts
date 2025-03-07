import { ApolloLink, RequestHandler } from '@apollo/client';
import { IInfraSettings } from '../../settings';

export type GetApolloLinkType = (settings: IInfraSettings) => ApolloLink;
export type GetRequestLinkType = (settings: IInfraSettings) => RequestHandler;
