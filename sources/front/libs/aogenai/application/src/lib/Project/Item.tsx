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
import { UserGroupGenerator } from '../UserStoryGroup';

type IProjectItemProps = IFormWithButtonsProps<IProjectDto>;

export const ProjectItem: FC<IProjectItemProps> = memo(
  ({ data, save, reset, remove, children, ...htmlAttributes }) => {
    const renderChildren = useCallback<
      NonNullable<IProjectItemProps['children']>
    >(
      (item, setItem) => {
        const IsEdition = Boolean(item?.id);
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
                  <CustomAccordion title="Editor">
                    <UserGroupGenerator projectId={item.id} />
                  </CustomAccordion>
                  {/* {item.stories&& <UserGroupEdit projectId={item.id} id={item.stories.id} reset={()=>item.stories!}/> */}
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
