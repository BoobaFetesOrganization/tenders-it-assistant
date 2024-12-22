import { Button, Grid2, Grid2Props } from '@mui/material';
import clsx from 'clsx';
import { memo, ReactNode } from 'react';
import { data } from 'react-router';
import { DataNotFound } from './DataNotFound';
import { Loading } from './Loading';

export interface ICustomFormProps extends Grid2Props {
  loading?: boolean;
  contentSpacing?: boolean;
  onSave?(): void;
  onReset?(): void;
  onRemove?(): void;
  actions?: ReactNode;
  text?: { save?: string; reset?: string; delete?: string };
}

export const CustomForm = memo(
  ({
    text,
    contentSpacing,
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
        className={clsx('custom-form', htmlAttributes.className)}
        sx={(theme) => ({
          margin: theme.spacing(1),
        })}
      >
        <Grid2 container alignItems="center" justifyContent="end" gap={2}>
          {actions}
          {onRemove && (
            <Button variant="contained" color="error" onClick={onRemove}>
              {text?.delete ?? 'Delete'}
            </Button>
          )}
        </Grid2>
        <Grid2
          container
          flex={1}
          direction="column"
          gap={2}
          sx={(theme) => ({ margin: theme.spacing(2, contentSpacing ? 2 : 0) })}
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
            {text?.reset ?? 'Reset'}
          </Button>
          <Button variant="contained" color="primary" onClick={onSave}>
            {text?.save ?? 'Save'}
          </Button>
        </Grid2>
      </Grid2>
    );
  }
);
