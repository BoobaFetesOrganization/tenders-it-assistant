import { getClient, getInfraSettings } from '@aogenai/infra';
import { ApolloProvider } from '@apollo/client';
import * as ReactDOM from 'react-dom/client';
import { App } from './app';
import './settings';
import { loadSettings } from './settings';
import './styles.scss';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const renderApp = async () => {
  await loadSettings();

  root.render(
    <ApolloProvider client={getClient(getInfraSettings())}>
      <App />
    </ApolloProvider>
  );
};
renderApp();
