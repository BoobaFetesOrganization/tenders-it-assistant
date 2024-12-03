import { newPage, newProjectDto } from '@aogenai/domain';
import {
  newPaginationParameter,
  useCreateUserStoryGroup,
  useGenerateUserStoryGroup,
  useProject,
  useUserStoryGroups,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { Button, Grid2, Tab, Tabs } from '@mui/material';
import { FC, memo, SyntheticEvent, useCallback, useState } from 'react';
import { UserStoryGroupEdit } from './Edit';

interface IUserGroupGeneratorProps {
  projectId: number;
}
export const UserGroupGenerator: FC<IUserGroupGeneratorProps> = memo(
  ({ projectId }) => {
    const [tab, setTab] = useState(0);
    const { data: { project } = { project: newProjectDto() } } = useProject({
      variables: { id: projectId },
      fetchPolicy: 'cache-first',
    });

    const { data: { groups } = { groups: newPage() } } = useUserStoryGroups({
      variables: { ...newPaginationParameter(), projectId },
    });
    const [create] = useCreateUserStoryGroup({ variables: { projectId } });
    const [generate] = useGenerateUserStoryGroup();
    const [validate] = useValidateUserStoryGroup();

    const onTabChange = useCallback((evt: SyntheticEvent, value: number) => {
      setTab(value);
    }, []);

    const editedGroup =
      tab === 0 ? project.stories : groups.data[tab - 1] ?? undefined;
    return (
      <Grid2 container flex={1} direction="column">
        <Grid2>
          <Button variant="outlined" color="secondary" onClick={() => create()}>
            Create
          </Button>
        </Grid2>
        <Grid2 container>
          <Grid2>
            <Tabs
              value={tab}
              onChange={onTabChange}
              variant="scrollable"
              scrollButtons="auto"
              aria-label="scrollable tabs for generated groups of user stories"
              orientation="vertical"
              sx={{ borderRight: 1, borderColor: 'divider' }}
            >
              <Tab label={`validated`} disabled={!project.stories} />
              {groups.data.map((group, index) => (
                <Tab label={`group ${index}`} key={group.id} />
              ))}
            </Tabs>
          </Grid2>
          <Grid2 container flex={1} sx={{ height: 500, overflow: 'auto' }}>
            {editedGroup && (
              <UserStoryGroupEdit
                projectId={projectId}
                groupId={editedGroup.id}
                actions={
                  <>
                    <Button
                      color="secondary"
                      onClick={() =>
                        generate({
                          variables: { projectId, id: editedGroup.id },
                        })
                      }
                    >
                      Generate
                    </Button>
                    <Button
                      color="primary"
                      onClick={() =>
                        validate({
                          variables: { projectId, id: editedGroup.id },
                        })
                      }
                    >
                      Validate
                    </Button>
                  </>
                }
              />
            )}
          </Grid2>
        </Grid2>
      </Grid2>
    );
  }
);
