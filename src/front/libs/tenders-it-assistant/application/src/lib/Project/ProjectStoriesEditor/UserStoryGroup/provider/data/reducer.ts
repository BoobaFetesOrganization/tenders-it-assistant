import {
  IUserStoryGroupDto,
  newTaskDto,
  newUserStoryDto,
} from '@tenders-it-assistant/domain';
import { Reducer } from 'react';
import { Action } from './Action';

export const reducer: Reducer<IUserStoryGroupDto, Action> = (state, action) => {
  switch (action.type) {
    case 'group:init':
      return action.payload;

    case 'request:update':
      return { ...state, request: { ...state.request, ...action.payload } };

    case 'story:create':
      return {
        ...state,
        userStories: [...state.userStories, newUserStoryDto()],
      };

    case 'story:update':
      return {
        ...state,
        userStories: state.userStories.map((item, i) =>
          i === action.payload.storyIndex
            ? { ...item, ...action.payload.entity }
            : item
        ),
      };

    case 'story:delete':
      return {
        ...state,
        userStories: state.userStories.filter(
          (_, i) => i !== action.payload.storyIndex
        ),
      };

    case 'task:create':
      return {
        ...state,
        userStories: state.userStories.map((story, storyIndex) =>
          storyIndex === action.payload.storyIndex
            ? { ...story, tasks: [...story.tasks, newTaskDto()] }
            : story
        ),
      };
    case 'task:update':
      return {
        ...state,
        userStories: state.userStories.map((story, storyIndex) =>
          storyIndex === action.payload.storyIndex
            ? {
                ...story,
                tasks: story.tasks.map((task, taskIndex) =>
                  taskIndex === action.payload.taskIndex
                    ? { ...task, ...action.payload.entity }
                    : task
                ),
              }
            : story
        ),
      };
    case 'task:delete':
      return {
        ...state,
        userStories: state.userStories.map((story, storyIndex) =>
          storyIndex === action.payload.storyIndex
            ? {
                ...story,
                tasks: story.tasks.filter(
                  (_, taskIndex) => taskIndex !== action.payload.taskIndex
                ),
              }
            : story
        ),
      };

    default:
      return state;
  }
};
