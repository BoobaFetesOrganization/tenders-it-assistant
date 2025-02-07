import {
  Grid2,
  Pagination,
  Paper,
  Table,
  TableBody,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';
import clsx from 'clsx';
import {
  Children,
  FC,
  HTMLAttributes,
  memo,
  PropsWithChildren,
  ReactElement,
} from 'react';
import { Loading } from '../Loading';
import {
  DataCell,
  DataContent,
  DataHead,
  DataPagination,
  Root,
} from './components';
import { DataPaginationChildren } from './components/DataPaginationChildren';
import { ColumnName } from './DataColumn';
import { PaginationName } from './DataPagination';
import { IDataColumnProps } from './IDataColumnProps';
import { IDataPaginationProps } from './IDataPaginationProps';

interface IDataTableProps<T>
  extends PropsWithChildren,
    HTMLAttributes<HTMLDivElement> {
  loading?: boolean;
  data: T[];
  sticky?: boolean;
  actions?: FC<T>;
}

export const DataTable = memo(
  <T,>({
    loading,
    data,
    sticky,
    actions: Actions,
    children: _children,
    ...htmlAttr
  }: IDataTableProps<T>) => {
    const elements = Children.toArray(_children) as ReactElement[];

    const pagination: ReactElement<IDataPaginationProps> = elements.filter(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (elt) => (elt?.type as any).displayName === PaginationName
    )[0];

    const columns: ReactElement<IDataColumnProps<T>>[] = elements.filter(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (elt) => (elt?.type as any).displayName === ColumnName
    );

    const hasActions =
      !!Actions || columns.some((child) => !!child.props.actions);

    return (
      <Root
        container
        {...htmlAttr}
        className={clsx('datatable', htmlAttr.className)}
      >
        {pagination && (
          <DataPagination>
            <DataPaginationChildren>
              {pagination.props.children}
            </DataPaginationChildren>
            <Grid2 flexGrow={0}>
              <Pagination
                count={Math.ceil(
                  (pagination.props.count ?? 0) /
                    pagination.props.maxItemPerPage
                )}
                siblingCount={2}
                variant="outlined"
                color="primary"
                size="small"
                disabled={
                  pagination.props.disabled !== undefined
                    ? pagination.props.disabled
                    : data.length === 0
                }
                onChange={pagination.props.onChange}
                showFirstButton
                showLastButton
              />
            </Grid2>
          </DataPagination>
        )}
        <DataContent container className="datatable__body">
          {loading && <Loading />}
          {!loading && (
            <TableContainer
              className="datatable-body__table"
              component={Paper}
              sx={{ overflow: 'auto', height: '100%' }}
            >
              <Table stickyHeader={sticky}>
                <TableHead>
                  <TableRow>
                    {columns.map(({ props: { fill, title } }, index) => (
                      <DataHead
                        key={index}
                        sx={{ width: (!!fill && '100%') || undefined }}
                      >
                        <Typography>{title}</Typography>
                      </DataHead>
                    ))}
                    {hasActions && (
                      <DataHead>
                        <Typography>Actions</Typography>
                      </DataHead>
                    )}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {data.map((item: T, index) => (
                    <TableRow key={index}>
                      {columns.map(
                        (
                          { props: { source, value, onSelect, fill } },
                          index
                        ) => (
                          <DataCell
                            key={index}
                            onClick={() => onSelect?.(item)}
                            sx={{
                              width: (!!fill && source && '100%') || undefined,
                              '&:hover': {
                                textDecoration: onSelect && 'underline',
                              },
                            }}
                          >
                            <Typography
                              fontWeight={(!!onSelect && 'bold') || undefined}
                            >
                              {(!!source && String(item[source])) ||
                                (value && value(item))}
                            </Typography>
                          </DataCell>
                        )
                      )}
                      {hasActions && (
                        <DataCell>
                          {Actions && <Actions key={index} {...item} />}
                          {columns.map(
                            ({ props: { actions: Actions } }, index) =>
                              !!Actions && <Actions key={index} {...item} />
                          )}
                        </DataCell>
                      )}
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </DataContent>
      </Root>
    );
  }
) as <T>(props: IDataTableProps<T>) => JSX.Element;
