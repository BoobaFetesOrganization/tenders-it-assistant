import { Box, Button } from '@mui/material';
import {
  HTMLAttributes,
  memo,
  ReactNode,
  useCallback,
  useEffect,
  useMemo,
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
}

const FormWithButtonsInternal = <T extends object>({
  data,
  save,
  reset,
  remove,
  children,
  ...htmlAttributes
}: IFormWithButtonsProps<T>) => {
  const [item, setItem] = useState<T>(data);
  const _children = useMemo(() => children || (() => null), [children]);

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
    <Box
      {...htmlAttributes}
      sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
    >
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        {_children(item, setItem)}
      </Box>
      <Box sx={{ display: 'flex', gap: 2, justifyContent: 'end' }}>
        {remove && (
          <Button variant="contained" color="error" onClick={onDelete}>
            Delete
          </Button>
        )}
        <Button variant="outlined" color="secondary" onClick={onReset}>
          Reset
        </Button>
        <Button variant="contained" color="primary" onClick={onSave}>
          Save
        </Button>
      </Box>
    </Box>
  );
};

export const FormWithButtons = memo(FormWithButtonsInternal) as <
  T extends object
>(
  props: IFormWithButtonsProps<T>
) => JSX.Element;
