import { Button, Grid2 } from '@mui/material';
import {
  HTMLAttributes,
  memo,
  ReactNode,
  useCallback,
  useEffect,
  useState,
} from 'react';
import { DataNotFound } from '../common';

export interface IFormWithButtonsProps<T extends object>
  extends Omit<HTMLAttributes<HTMLDivElement>, 'children'> {
  data: T;
  save?(data: T): void;
  reset?(): T;
  remove?(item: T): void;
  children?(item: T, setItem: (value: T) => void): ReactNode;
  actions?: ReactNode;
}

export type FormWithButtonsChildren<T extends object> = NonNullable<
  IFormWithButtonsProps<T>['children']
>;

const FormWithButtonsInternal = <T extends object>({
  data,
  save,
  reset,
  remove,
  actions,
  children,
  ...htmlAttributes
}: IFormWithButtonsProps<T>) => {
  const [item, setItem] = useState<T>(data);

  useEffect(() => {
    setItem(data);
  }, [data]);

  const onSave = useCallback(() => {
    save?.(item);
  }, [item, save]);

  const onReset = useCallback(() => {
    reset && setItem(reset());
  }, [reset]);

  const onDelete = useCallback(() => {
    remove?.(data);
  }, [data, remove]);

  if (!data) return <DataNotFound />;

  return (
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
        {remove && (
          <Button variant="contained" color="error" onClick={onDelete}>
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
        {children?.(item, setItem)}
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
};

export const FormWithButtons = memo(FormWithButtonsInternal) as <
  T extends object
>(
  props: IFormWithButtonsProps<T>
) => JSX.Element;
