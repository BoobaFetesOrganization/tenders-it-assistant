import { IUserStoryDto, newUserStoryDto } from '@aogenai/domain';
import { useUserStory } from '@aogenai/infra';
import {
  createContext,
  Dispatch,
  FC,
  memo,
  PropsWithChildren,
  useContext,
  useEffect,
  useReducer,
} from 'react';
import { Loading } from '../../../common';
import { Action, initStory, reducer } from './data';
import { IUserStoryProps } from './IUserStoryProps';

// Définir le type pour le contexte
interface StoryContextType {
  story: IUserStoryDto;
  dispatch: Dispatch<Action>;
}

// Créer le contexte
const StoryContext = createContext<StoryContextType | undefined>(undefined);

// Créer le provider
export const StoryProvider: FC<PropsWithChildren<IUserStoryProps>> = memo(
  ({ projectId, groupId, storyId, children }) => {
    const [story, dispatch] = useReducer(reducer, newUserStoryDto());

    const {
      called,
      loading,
      data: { story: initial } = { story: newUserStoryDto() },
    } = useUserStory({
      variables: { projectId, groupId, id: storyId },
    });

    useEffect(() => {
      if (called && !loading && initial) {
        initStory(dispatch, initial);
      }
    }, [called, initial, loading]);

    return loading ? (
      <Loading />
    ) : (
      <StoryContext.Provider value={{ story, dispatch }}>
        {children}
      </StoryContext.Provider>
    );
  }
);

// Hook pour utiliser le contexte
export const useStoryContext = () => useContext(StoryContext);
