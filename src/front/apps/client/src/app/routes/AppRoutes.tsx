import { FC, memo } from 'react';
import { useRoutes } from 'react-router';
import { routes } from './routes';

export const AppRoutes: FC = memo(() => {
  const element = useRoutes(routes);
  return element;
});
