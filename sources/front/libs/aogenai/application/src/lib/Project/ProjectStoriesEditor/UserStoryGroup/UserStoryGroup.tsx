import { IUserStoryGroupDto, newUserStoryGroupDto } from '@aogenai/domain';
import {
  useDeleteUserStoryGroup,
  useGenerateUserStoryGroup,
  useUpdateUserStoryGroup,
  useUserStoryGroup,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { Button, Grid2 } from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import {
  CustomAccordion,
  FormWithButtons,
  FormWithButtonsChildren,
  Loading,
} from '../../../common';
import { onValueChange } from '../../../tools';
import { UserStory } from '../UserStory';
import { UserGroupRequest } from './UserGroupRequest';

interface IUserStoryGroupProps {
  projectId: number;
  groupId: number;
  onSaved?: (item: IUserStoryGroupDto) => void;
  onDeleted?: (item: IUserStoryGroupDto) => void;
}
export const UserStoryGroup: FC<IUserStoryGroupProps> = memo(
  ({ projectId, groupId, onSaved, onDeleted }) => {
    const { loading, data: { group } = { group: newUserStoryGroupDto() } } =
      useUserStoryGroup({
        variables: { projectId, id: groupId },
      });
    const [update] = useUpdateUserStoryGroup({
      onCompleted({ group }) {
        onSaved?.(group);
      },
    });
    const [remove] = useDeleteUserStoryGroup({
      variables: { projectId, id: groupId },
      onCompleted({ group }) {
        onDeleted?.(group);
      },
    });

    const reset = useCallback(() => group, [group]);

    const [generate] = useGenerateUserStoryGroup();

    const [validate] = useValidateUserStoryGroup();

    const [requestOpen, setRequestOpen] = useState(true);

    const save = useCallback(
      (input: IUserStoryGroupDto) => {
        update({ variables: { projectId, input } });
      },
      [projectId, update]
    );

    const onGenerate = useCallback(() => {
      generate({ variables: { projectId, id: group.id } });
    }, [generate, projectId, group.id]);

    const onValidate = useCallback(() => {
      validate({ variables: { projectId, id: group.id } });
    }, [validate, projectId, group.id]);

    const renderChildren = useCallback<
      FormWithButtonsChildren<IUserStoryGroupDto>
    >(
      (item, setItem) => (
        <Grid2 container flex={1} direction="column">
          <CustomAccordion
            title="Request"
            open={requestOpen}
            onChange={setRequestOpen}
          >
            <UserGroupRequest
              request={item.request}
              onChanged={onValueChange({
                item,
                setItem,
                property: 'request',
              })}
            />
          </CustomAccordion>
          <Grid2
            container
            direction="column"
            spacing={2}
            id="userstory-collection"
          >
            {group.userStories.map((story) => (
              <UserStory
                key={story.id}
                projectId={projectId}
                groupId={groupId}
                storyId={story.id}
              />
            ))}
          </Grid2>
        </Grid2>
      ),
      [group.userStories, groupId, projectId, requestOpen]
    );

    return loading ? (
      <Loading />
    ) : (
      <FormWithButtons
        data={group}
        save={save}
        reset={reset}
        remove={remove}
        actions={
          <>
            <Button color="primary" onClick={onGenerate}>
              Generate
            </Button>
            <Button color="primary" onClick={onValidate}>
              Validate
            </Button>
          </>
        }
      >
        {renderChildren}
      </FormWithButtons>
    );
  }
);
