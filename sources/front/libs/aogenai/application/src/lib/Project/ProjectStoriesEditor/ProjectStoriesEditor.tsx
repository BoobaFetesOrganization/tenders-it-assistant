import { newPage, newUserStoryGroupDto } from '@aogenai/domain';
import {
  newPaginationParameter,
  useCreateUserStoryGroup,
  useUserStoryGroups,
} from '@aogenai/infra';
import { Button, Grid2, Tab, Tabs } from '@mui/material';
import { FC, memo, SyntheticEvent, useCallback, useState } from 'react';
import { UserStoryGroup } from './UserStoryGroup';

interface IProjectStoriesEditorProps {
  projectId: number;
}
export const ProjectStoriesEditor: FC<IProjectStoriesEditorProps> = memo(
  ({ projectId }) => {
    const [tab, setTab] = useState(0);

    const { data: { groups } = { groups: newPage() } } = useUserStoryGroups({
      variables: { ...newPaginationParameter(), projectId },
    });

    const [create] = useCreateUserStoryGroup({ variables: { projectId } });
    const onCreate = useCallback(() => {
      create();
    }, [create]);

    const onTabChange = useCallback((evt: SyntheticEvent, value: number) => {
      setTab(value);
    }, []);

    const current = groups.data[tab] ?? newUserStoryGroupDto();
    return (
      <Grid2 container flex={1} direction="column">
        <Grid2 container alignItems="end">
          <Button variant="outlined" color="secondary" onClick={onCreate}>
            Create
          </Button>
        </Grid2>
        <Grid2 container>
          <Tabs
            value={tab}
            onChange={onTabChange}
            variant="scrollable"
            scrollButtons="auto"
            aria-label="scrollable tabs for generated groups of user stories"
          >
            {groups.data.map((group, index) => (
              <Tab label={`group ${index}`} key={group.id} />
            ))}
          </Tabs>
        </Grid2>
        <Grid2 container flex={1} sx={{ overflow: 'auto' }}>
          <UserStoryGroup projectId={current.projectId} groupId={current.id} />
        </Grid2>
      </Grid2>
    );
  }
);
