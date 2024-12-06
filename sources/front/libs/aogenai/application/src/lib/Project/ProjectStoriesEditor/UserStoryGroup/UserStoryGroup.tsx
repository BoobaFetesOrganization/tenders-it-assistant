import { IUserStoryGroupDto } from '@aogenai/domain';
import {
  useDeleteUserStoryGroup,
  useGenerateUserStoryGroup,
  useUpdateUserStoryGroup,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { Button, Grid2, Typography } from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import {
  CustomAccordion,
  FormWithButtons,
  FormWithButtonsChildren,
} from '../../../common';
import { UserStory } from '../UserStory';
import { UserGroupRequest } from './UserGroupRequest';
import { useUserStoryGroupData } from './provider';

export const UserStoryGroup: FC = memo(() => {
  const { group, story, onDeleted } = useUserStoryGroupData();
  const [requestOpen, setRequestOpen] = useState(true);

  const [update] = useUpdateUserStoryGroup();
  const [remove] = useDeleteUserStoryGroup({
    variables: { projectId: group.projectId, id: group.id },
    onCompleted({ group }) {
      onDeleted?.(group);
    },
  });

  const reset = useCallback(() => group, [group]);

  const [generate] = useGenerateUserStoryGroup();

  const [validate] = useValidateUserStoryGroup();

  const save = useCallback(
    (input: IUserStoryGroupDto) => {
      update({ variables: { projectId: group.projectId, input } });
    },
    [group.projectId, update]
  );

  const onGenerate = useCallback(() => {
    generate({ variables: { projectId: group.projectId, id: group.id } });
  }, [generate, group.projectId, group.id]);

  const onValidate = useCallback(() => {
    validate({ variables: { projectId: group.projectId, id: group.id } });
  }, [validate, group.projectId, group.id]);

  const renderChildren = useCallback<
    FormWithButtonsChildren<IUserStoryGroupDto>
  >(
    (item, setItem) => {
      return (
        <>
          <CustomAccordion
            title="Request"
            open={requestOpen}
            onChange={setRequestOpen}
          >
            <UserGroupRequest />
          </CustomAccordion>

          <Grid2
            container
            direction="column"
            spacing={2}
            id="userstory-collection"
          >
            <Typography variant="h4">User stories</Typography>
          </Grid2>
          <Grid2
            container
            direction="column"
            spacing={2}
            id="userstory-collection"
          >
            {group.userStories.map((story, storyIndex) => (
              <UserStory
                key={`${storyIndex}-${story.id}`}
                storyIndex={storyIndex}
              />
            ))}
          </Grid2>
          <Grid2 container justifyContent="end" spacing={2}>
            <Button variant="outlined" color="primary" onClick={story.create}>
              Add User story
            </Button>
          </Grid2>
        </>
      );
    },
    [group.userStories, requestOpen, story.create]
  );

  return (
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
});
