import { newPage, newProjectDto, newUserStoryGroupDto } from '@aogenai/domain';
import {
  newPaginationParameter,
  useCreateUserStoryGroup,
  useProject,
  useUserStoryGroups,
} from '@aogenai/infra';
import { Button, Grid2, Tab, Tabs } from '@mui/material';
import { FC, memo, SyntheticEvent, useCallback, useState } from 'react';
import { Loading } from '../../common';
import { UserStoryGroup } from './UserStoryGroup';

interface IProjectStoriesEditorProps {
  projectId: number;
}
export const ProjectStoriesEditor: FC<IProjectStoriesEditorProps> = memo(
  ({ projectId }) => {
    const [tab, setTab] = useState(0);

    const {
      data: { project: { selectedGroup } } = {
        project: newProjectDto(),
      },
    } = useProject({ variables: { id: projectId } });

    const { data: { groups } = { groups: newPage() } } = useUserStoryGroups({
      variables: { ...newPaginationParameter(), projectId },
    });

    const [create, { loading: createLoading }] = useCreateUserStoryGroup({
      variables: { projectId },
    });
    const onCreate = useCallback(() => {
      create();
    }, [create]);

    const onTabChange = useCallback((evt: SyntheticEvent, value: number) => {
      setTab(value);
    }, []);

    const current = groups.data[tab] ?? newUserStoryGroupDto();
    return (
      <Grid2 container flex={1} direction="column">
        <Grid2 container alignItems="end" spacing={2}>
          <Grid2>
            <Button variant="outlined" color="secondary" onClick={onCreate}>
              Create
            </Button>
          </Grid2>
          {createLoading && (
            <Grid2>
              <Loading showImmediately />
            </Grid2>
          )}
        </Grid2>
        {!!groups.data.length && (
          <>
            <Grid2 container>
              <Tabs
                value={tab}
                onChange={onTabChange}
                variant="scrollable"
                scrollButtons="auto"
                aria-label="scrollable tabs for generated groups of user stories"
              >
                {groups.data.map((group, index) => (
                  <Tab
                    label={
                      group.id === selectedGroup?.id
                        ? 'validated'
                        : `group ${index}`
                    }
                    key={group.id}
                    sx={{
                      color:
                        group.id === selectedGroup?.id
                          ? 'primary.main'
                          : 'inherit',
                    }}
                  />
                ))}
              </Tabs>
            </Grid2>
            <Grid2 container flex={1} sx={{ overflow: 'auto' }}>
              <UserStoryGroup
                projectId={current.projectId}
                groupId={current.id}
              />
            </Grid2>
          </>
        )}
      </Grid2>
    );
  }
);
