import {
  useDeleteUserStoryGroup,
  useGenerateUserStoryGroup,
  useUpdateUserStoryGroup,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { Button, Grid2, Typography } from '@mui/material';
import { FC, memo, useCallback, useState } from 'react';
import { CustomAccordion, CustomForm } from '../../../common';
import { UserStory } from '../UserStory';
import { UserGroupRequest } from './UserGroupRequest';
import { useUserStoryGroupData } from './provider';

export const UserStoryGroup: FC = memo(() => {
  const { group, story, onDeleted, reset } = useUserStoryGroupData();
  const [requestOpen, setRequestOpen] = useState(true);

  const [update] = useUpdateUserStoryGroup({
    onCompleted({ group }) {
      reset(group);
    },
  });
  const [remove] = useDeleteUserStoryGroup({
    variables: { projectId: group.projectId, id: group.id },
    onCompleted({ group }) {
      onDeleted?.(group);
    },
  });

  const [generate] = useGenerateUserStoryGroup();

  const [validate] = useValidateUserStoryGroup();

  const onSave = useCallback(() => {
    update({ variables: { projectId: group.projectId, input: group } });
  }, [group, update]);

  const onRemove = useCallback(() => {
    remove({ variables: { projectId: group.projectId, id: group.id } });
  }, [group.id, group.projectId, remove]);

  const onGenerate = useCallback(() => {
    generate({ variables: { projectId: group.projectId, id: group.id } });
  }, [generate, group.projectId, group.id]);

  const onValidate = useCallback(() => {
    validate({ variables: { projectId: group.projectId, id: group.id } });
  }, [validate, group.projectId, group.id]);

  return (
    <CustomForm
      onSave={onSave}
      onReset={reset}
      onRemove={onRemove}
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
      <CustomAccordion
        title="Request"
        open={requestOpen}
        onChange={setRequestOpen}
      >
        <UserGroupRequest />
      </CustomAccordion>

      <Grid2 container direction="column" spacing={2} id="userstory-collection">
        <Typography variant="h4">User stories</Typography>
      </Grid2>
      <Grid2 container direction="column" spacing={2} id="userstory-collection">
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
    </CustomForm>
  );
});
