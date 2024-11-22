import { memo } from 'react';
import { SearchAppBar } from './HeaderBar';

export const App = memo(() => {
  return (
    <div>
      <SearchAppBar />
    </div>
  );
});
