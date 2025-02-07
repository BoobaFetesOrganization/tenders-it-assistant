import {
  DocumentConverter,
  ProjectCreate,
  ProjectEdit,
} from '@aogenai/application';
import { IDocumentDto, IProjectDto } from '@aogenai/domain';
import { saveAs } from 'file-saver';
import { FC, memo, useCallback } from 'react';
import { useNavigate } from 'react-router';
import { routeMapping } from './routeMapping';
import { useProjectParams } from './useProjectParams';

const converter = new DocumentConverter();

export const ProjectItemWrapper: FC = memo(() => {
  const navigate = useNavigate();
  const { id } = useProjectParams();

  const navigateToEdit = useCallback(
    (item: IProjectDto) => navigate(routeMapping.url({ id: item.id }).to),
    [navigate]
  );
  const navigateToList = useCallback(
    () => navigate(routeMapping.url().to),
    [navigate]
  );
  const navigateToEditor = useCallback(
    (item: IProjectDto) =>
      navigate(routeMapping.urlToEditor({ id: item.id }).to),
    [navigate]
  );

  const onDocumentDownloaded = useCallback((document: IDocumentDto) => {
    const { id, name } = document;
    saveAs(converter.toBlob(document), `${id}-${name}`);
  }, []);

  return id === 0 ? (
    <ProjectCreate onCreated={navigateToEdit} />
  ) : (
    <ProjectEdit
      id={id}
      onDeleted={navigateToList}
      onDocumentDonwloaded={onDocumentDownloaded}
      onUserStoryEditorCLick={navigateToEditor}
    />
  );
});
