import { Button, Grid2, Typography } from '@mui/material';
import { FC, memo } from 'react';
import { CustomAccordion, CustomForm, Loading } from '../../../common';
import { UserStory } from '../UserStory';
import { UserGroupRequest } from './UserGroupRequest';
import { useUserStoryGroupBehavior } from './useUserStoryGroupBehavior';

export const UserStoryGroup: FC = memo(() => {
  const {
    group,
    story,
    reset,
    total,
    requestOpen,
    setRequestOpen,
    onSaveRequest,
    userstoryOpen,
    setUserstoryOpen,
    onSaveUserStories,
    generateLoading,
    onGenerate,
    validateLoading,
    onValidate,
    onRemove,
  } = useUserStoryGroupBehavior();

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
          <CustomForm
            onSave={onSaveRequest}
            onReset={reset}
            gap={0}
            text={{ save: 'Save & Generate' }}
          >
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
                {`Total Gemini : ${total.geminiCost}`}
              </Typography>
              <Typography variant="body1">
                {`Total Human : ${total.cost}`}
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
