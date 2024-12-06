import { Dispatch } from 'react';
import {
  Action,
  CreateTaskAction,
  DeleteStoryAction,
  DeleteTaskAction,
  InitGroupAction,
  UpdateRequestAction,
  UpdateStoryAction,
  UpdateTaskAction,
} from './Action';

export const initGroup = (
  dispatch: Dispatch<Action>,
  payload: InitGroupAction['payload']
) => {
  dispatch({ type: 'group:init', payload });
};

export const updateRequest =
  (dispatch: Dispatch<Action>) => (payload: UpdateRequestAction['payload']) => {
    dispatch({ type: 'request:update', payload });
  };

export const createStory = (dispatch: Dispatch<Action>) => () => {
  dispatch({ type: 'story:create' });
};

export const updateStory =
  (dispatch: Dispatch<Action>) => (payload: UpdateStoryAction['payload']) => {
    dispatch({ type: 'story:update', payload });
  };

export const deleteStory =
  (dispatch: Dispatch<Action>) => (payload: DeleteStoryAction['payload']) => {
    dispatch({ type: 'story:delete', payload });
  };

export const createTask =
  (dispatch: Dispatch<Action>) => (payload: CreateTaskAction['payload']) => {
    dispatch({ type: 'task:create', payload });
  };

export const updateTask =
  (dispatch: Dispatch<Action>) => (payload: UpdateTaskAction['payload']) => {
    dispatch({ type: 'task:update', payload });
  };

export const deleteTask =
  (dispatch: Dispatch<Action>) => (payload: DeleteTaskAction['payload']) => {
    dispatch({ type: 'task:delete', payload });
  };
