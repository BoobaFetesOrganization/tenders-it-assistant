import { Button, Grid2, Grid2Props } from '@mui/material';
import { memo, ReactNode } from 'react';
import { data } from 'react-router';
import { DataNotFound } from './DataNotFound';
import { Loading } from './Loading';

export interface ICustomFormProps extends Grid2Props {
  loading?: boolean;
  onSave?(): void;
  onReset?(): void;
  onRemove?(): void;
  actions?: ReactNode;
}

export const CustomForm = memo(
  ({
    loading,
    actions,
    onSave,
    onReset,
    onRemove,
    children,
    ...htmlAttributes
  }: ICustomFormProps) => {
    return loading ? (
      <Loading />
    ) : !data ? (
      <DataNotFound />
    ) : (
      <Grid2
        container
        flex={1}
        direction="column"
        gap={2}
        {...htmlAttributes}
        sx={(theme) => ({
          margin: theme.spacing(1),
        })}
      >
        <Grid2 container alignItems="center" justifyContent="end" gap={2}>
          {actions}
          {onRemove && (
            <Button variant="contained" color="error" onClick={onRemove}>
              Delete
            </Button>
          )}
        </Grid2>
        <Grid2
          container
          flex={1}
          direction="column"
          gap={2}
          sx={(theme) => ({ margin: theme.spacing(2) })}
        >
          {children}
        </Grid2>
        <Grid2
          container
          flex={0}
          alignItems="center"
          justifyContent="end"
          gap={2}
        >
          <Button variant="outlined" color="secondary" onClick={onReset}>
            Reset
          </Button>
          <Button variant="contained" color="primary" onClick={onSave}>
            Save
          </Button>
        </Grid2>
      </Grid2>
    );
  }
);
