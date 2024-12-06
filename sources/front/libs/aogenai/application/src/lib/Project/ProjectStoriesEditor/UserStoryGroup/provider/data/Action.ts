import {
  ITaskDto,
  IUserStoryDto,
  IUserStoryGroupDto,
  IUserStoryPromptDto,
} from '@aogenai/domain';

type ActionBase<T extends string, P = undefined> = P extends undefined
  ? { type: T }
  : { type: T; payload: P };

export type InitGroupAction = ActionBase<'group:init', IUserStoryGroupDto>;

export type UpdateRequestAction = ActionBase<
  'request:update',
  Partial<IUserStoryPromptDto>
>;

export type CreateStoryAction = ActionBase<'story:create'>;
export type UpdateStoryAction = ActionBase<
  'story:update',
  { storyIndex: number; entity: Partial<IUserStoryDto> }
>;
export type DeleteStoryAction = ActionBase<
  'story:delete',
  { storyIndex: number }
>;

export type CreateTaskAction = ActionBase<
  'task:create',
  { storyIndex: number }
>;
export type UpdateTaskAction = ActionBase<
  'task:update',
  { storyIndex: number; taskIndex: number; entity: Partial<ITaskDto> }
>;
export type DeleteTaskAction = ActionBase<
  'task:delete',
  { storyIndex: number; taskIndex: number }
>;

export type Action =
  | InitGroupAction
  | UpdateRequestAction
  | CreateStoryAction
  | UpdateStoryAction
  | DeleteStoryAction
  | CreateTaskAction
  | UpdateTaskAction
  | DeleteTaskAction;
