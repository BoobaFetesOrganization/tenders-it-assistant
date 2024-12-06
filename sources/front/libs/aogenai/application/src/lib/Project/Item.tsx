import { IDocumentDto, IProjectDto } from '@aogenai/domain';
import { Box, Button, TextField } from '@mui/material';
import { FC, memo } from 'react';
import { CustomAccordion, CustomForm, ICustomFormProps } from '../common';
import { DocumentCollection } from '../Document';
import { onPropertyChange } from '../tools';
import { UserStoryGroup } from '../UserStoryGroup';

export interface IProjectItemProps extends ICustomFormProps {
  item: IProjectDto;
  setItem(value: IProjectDto): void;
  onDocumentDonwloaded?: (document: IDocumentDto) => void;
  onUserStoryEditorCLick?: (item: IProjectDto) => void;
}

export const ProjectItem: FC<IProjectItemProps> = memo(
  ({
    loading,
    item,
    setItem,
    onSave,
    onReset,
    onRemove,
    children,
    onDocumentDonwloaded,
    onUserStoryEditorCLick,
    ...htmlAttributes
  }) => {
    const IsEdition = Boolean(item?.id);

    return (
      <CustomForm
        loading={loading}
        onSave={onSave}
        onReset={onReset}
        onRemove={onRemove}
        {...htmlAttributes}
      >
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
              <DocumentCollection
                projectId={item.id}
                onDownloaded={onDocumentDonwloaded}
              />
            </CustomAccordion>
            {onUserStoryEditorCLick && (
              <Box sx={{ display: 'flex', justifyContent: 'end' }}>
                <Button
                  variant="outlined"
                  color="primary"
                  onClick={() => onUserStoryEditorCLick(item)}
                >
                  User stories Editor
                </Button>
              </Box>
            )}
            {item.selectedGroup && (
              <UserStoryGroup
                projectId={item.id}
                groupId={item.selectedGroup.id}
              />
            )}
          </>
        )}
      </CustomForm>
    );
  }
);
