import { getClient } from '@aogenai/infra';
import { ApolloProvider } from '@apollo/client';
import * as ReactDOM from 'react-dom/client';
import { App } from './app';
import { getSettings } from './app/settings';
import './styles.scss';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <ApolloProvider client={getClient(getSettings('infra'))}>
    <App />
  </ApolloProvider>
);
