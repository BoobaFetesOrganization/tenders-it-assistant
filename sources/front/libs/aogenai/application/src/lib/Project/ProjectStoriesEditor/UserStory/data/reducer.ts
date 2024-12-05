import { IUserStoryDto, newTaskDto, newUserStoryDto } from '@aogenai/domain';
import { Reducer } from 'react';
import { Action } from './Action';

export const reducer: Reducer<IUserStoryDto, Action> = (state, action) => {
  switch (action.type) {
    case 'story:init':
      return newUserStoryDto(action.payload);

    case 'story:update':
      return { ...state, ...action.payload };

    case 'task:create':
      return {
        ...state,
        tasks: [...state.tasks, newTaskDto()],
      };
    case 'task:update':
      return {
        ...state,
        tasks: state.tasks.map((item, index) =>
          index && action.payload.index && item.id === action.payload.entity.id
            ? { ...item, ...action.payload.entity }
            : item
        ),
      };
    case 'task:delete':
      return {
        ...state,
        tasks: state.tasks.filter((item) => item.id !== action.payload),
      };

    default:
      return state;
  }
};
