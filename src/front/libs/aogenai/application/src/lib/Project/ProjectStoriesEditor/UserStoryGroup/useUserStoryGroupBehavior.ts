import {
  useDeleteUserStoryGroup,
  useGenerateUserStoryGroup,
  useUpdateUserStoryGroup,
  useUpdateUserStoryGroupRequest,
  useValidateUserStoryGroup,
} from '@aogenai/infra';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useTotalCost } from '../../../common';
import { useUserStoryGroupData } from './provider';

export const useUserStoryGroupBehavior = () => {
  const { group, story, onDeleted, reset } = useUserStoryGroupData();
  const [requestOpen, setRequestOpen] = useState(false);
  const [userstoryOpen, setUserstoryOpen] = useState(false);

  useEffect(() => {
    setRequestOpen(group.userStories.length === 0);
    setUserstoryOpen(group.userStories.length > 0);
  }, [group.userStories.length]);

  const total = useTotalCost(group);

  const [updateRequest] = useUpdateUserStoryGroupRequest({
    onCompleted() {
      reset();
    },
  });
  const [update] = useUpdateUserStoryGroup();
  const [remove] = useDeleteUserStoryGroup({
    variables: { projectId: group.projectId, id: group.id },
    onCompleted({ group }) {
      onDeleted?.(group);
    },
  });

  const [generate, { loading: generateLoading }] = useGenerateUserStoryGroup();

  const [validate, { loading: validateLoading }] = useValidateUserStoryGroup();

  const onSaveRequest = useCallback(() => {
    updateRequest({
      variables: {
        projectId: group.projectId,
        groupId: group.id,
        input: group.request,
      },
    });
  }, [group, updateRequest]);
  const onSave = useCallback(() => {
    update({
      variables: { projectId: group.projectId, input: group },
    });
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

  return useMemo(
    () => ({
      group,
      story,
      reset,
      total,
      requestOpen,
      setRequestOpen,
      onSaveRequest,
      userstoryOpen,
      setUserstoryOpen,
      onSave,
      generateLoading,
      onGenerate,
      validateLoading,
      onValidate,
      onRemove,
    }),
    [
      generateLoading,
      group,
      onGenerate,
      onRemove,
      onSaveRequest,
      onSave,
      onValidate,
      requestOpen,
      reset,
      story,
      total,
      userstoryOpen,
      validateLoading,
    ]
  );
};
