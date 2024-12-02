import { IProjectDto } from '@aogenai/domain';
import { TextField } from '@mui/material';
import { FC, memo, useCallback } from 'react';
import {
  CustomAccordion,
  FormWithButtons,
  IFormWithButtonsProps,
} from '../common';
import { DocumentCollection } from '../Document';
import { onPropertyChange } from '../tools';
import { UserStoryGroupCollection } from '../UserStoryGroup';

type IProjectItemProps = IFormWithButtonsProps<IProjectDto>;

export const ProjectItem: FC<IProjectItemProps> = memo(
  ({ data, save, reset, remove, children, ...htmlAttributes }) => {
    const renderChildren = useCallback<
      NonNullable<IProjectItemProps['children']>
    >(
      (item, setItem) => {
        const IsEdition = Boolean(data?.id);
        return (
          <>
            <TextField
              label="Name"
              name="Name"
              value={item.name}
              onChange={onPropertyChange({ item, setItem, property: 'name' })}
              variant="outlined"
            />
            {IsEdition && (
              <>
                <CustomAccordion title="Documents">
                  <DocumentCollection projectId={item.id} />
                </CustomAccordion>
                <CustomAccordion title="User stories">
                  <UserStoryGroupCollection projectId={item.id} />
                </CustomAccordion>
              </>
            )}
            {children?.(item, setItem)}
          </>
        );
      },
      [children, data?.id]
    );

    return (
      <FormWithButtons
        data={data}
        save={save}
        reset={reset}
        remove={remove}
        {...htmlAttributes}
      >
        {renderChildren}
      </FormWithButtons>
    );
  }
);
