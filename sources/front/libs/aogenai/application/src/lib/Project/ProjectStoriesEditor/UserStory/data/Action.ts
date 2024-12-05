import { ITaskDto, IUserStoryDto } from '@aogenai/domain';

type ActionBase<T extends string, P = undefined> = P extends undefined
  ? { type: T }
  : { type: T; payload: P };

export type InitStoryAction = ActionBase<'story:init', IUserStoryDto>;
export type UpdateStoryAction = ActionBase<
  'story:update',
  Partial<IUserStoryDto>
>;
export type DeleteStoryAction = ActionBase<'story:delete', number>;

export type CreateTaskAction = ActionBase<'task:create'>;
export type UpdateTaskAction = ActionBase<
  'task:update',
  { index: number; entity: Partial<ITaskDto> }
>;
export type DeleteTaskAction = ActionBase<'task:delete', number>;

export type Action =
  | InitStoryAction
  | UpdateStoryAction
  | DeleteStoryAction
  | CreateTaskAction
  | UpdateTaskAction
  | DeleteTaskAction;

export const initStory = (
  dispatch: React.Dispatch<Action>,
  payload: IUserStoryDto
) => {
  dispatch({ type: 'story:init', payload });
};
export const updateStory =
  (dispatch: React.Dispatch<Action>) => (payload: Partial<IUserStoryDto>) => {
    dispatch({ type: 'story:update', payload });
  };

export const deleteStory =
  (dispatch: React.Dispatch<Action>) => (payload: number) => {
    dispatch({ type: 'story:delete', payload });
  };

export const createTask = (dispatch: React.Dispatch<Action>) => () => {
  dispatch({ type: 'task:create' });
};

export const updateTask =
  (dispatch: React.Dispatch<Action>) =>
  (payload: { index: number; entity: Partial<ITaskDto> }) => {
    dispatch({ type: 'task:update', payload });
  };

export const deleteTask =
  (dispatch: React.Dispatch<Action>) => (payload: number) => {
    dispatch({ type: 'task:delete', payload });
  };
