import {
  useDeleteUserStoryGroup,
  useGenerateUserStoryGroup,
  useUpdateUserStoryGroupRequest,
  useUpdateUserStoryGroupUserStories,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { Button, Grid2, Typography } from '@mui/material';
import { FC, memo, useCallback, useEffect, useState } from 'react';
import {
  CustomAccordion,
  CustomForm,
  Loading,
  useTotalCost,
} from '../../../common';
import { UserStory } from '../UserStory';
import { UserGroupRequest } from './UserGroupRequest';
import { useUserStoryGroupData } from './provider';

export const UserStoryGroup: FC = memo(() => {
  const { group, story, onDeleted, reset } = useUserStoryGroupData();
  const [requestOpen, setRequestOpen] = useState(false);
  const [userstoryOpen, setUserstoryOpen] = useState(false);

  useEffect(() => {
    setRequestOpen(group.userStories.length === 0);
    setUserstoryOpen(group.userStories.length > 0);
  }, [group.userStories.length]);
  const { totalCost, totalGeminiCost } = useTotalCost(group);

  const [updateRequest] = useUpdateUserStoryGroupRequest({
    onCompleted({ group }) {
      reset(group);
      generate({ variables: { projectId: group.projectId, id: group.id } });
    },
  });
  const [updateUserStories] = useUpdateUserStoryGroupUserStories({
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

  const [generate, { loading: generateLoading }] = useGenerateUserStoryGroup();

  const [validate, { loading: validateLoading }] = useValidateUserStoryGroup();

  const onSaveRequest = useCallback(() => {
    updateRequest({ variables: { projectId: group.projectId, input: group } });
  }, [group, updateRequest]);
  const onSaveUserStories = useCallback(() => {
    updateUserStories({
      variables: { projectId: group.projectId, input: group },
    });
  }, [group, updateUserStories]);

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
    <Grid2 container flex={1} direction="column">
      <Grid2 container alignItems="center" justifyContent="end" gap={2}>
        <Grid2>
          {generateLoading ? (
            <Loading showImmediately />
          ) : (
            <Button
              color="primary"
              onClick={onGenerate}
              disabled={validateLoading}
            >
              Generate
            </Button>
          )}
        </Grid2>
        <Grid2>
          {validateLoading ? (
            <Loading />
          ) : (
            <Button
              color="primary"
              onClick={onValidate}
              disabled={generateLoading}
            >
              Validate
            </Button>
          )}
        </Grid2>
        <Grid2>
          <Button
            variant="contained"
            color="error"
            onClick={onRemove}
            disabled={generateLoading || validateLoading}
          >
            Delete
          </Button>
        </Grid2>
      </Grid2>
      <Grid2 container direction="column" gap={2} margin={2}>
        <CustomAccordion
          title="Request"
          open={requestOpen}
          onChange={setRequestOpen}
        >
          <CustomForm onSave={onSaveRequest} onReset={reset} gap={0}>
            <UserGroupRequest />
          </CustomForm>
        </CustomAccordion>
      </Grid2>
      <Grid2 container direction="column" gap={2} margin={2}>
        <CustomAccordion
          title="User stories"
          open={userstoryOpen}
          onChange={setUserstoryOpen}
        >
          <Grid2
            container
            spacing={2}
            id="userstory-collection"
            direction="column"
          >
            <Grid2 container flexGrow={0} justifyContent="end">
              <Typography variant="body1">
                {`Total Gemini : ${totalGeminiCost}`}
              </Typography>
              <Typography variant="body1">
                {`Total Human : ${totalCost}`}
              </Typography>
            </Grid2>
          </Grid2>
          <CustomForm onSave={onSaveUserStories} onReset={reset}>
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
          </CustomForm>
        </CustomAccordion>
      </Grid2>
    </Grid2>
  );
});
