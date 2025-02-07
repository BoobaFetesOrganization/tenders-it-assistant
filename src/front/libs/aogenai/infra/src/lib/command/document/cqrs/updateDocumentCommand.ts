import { getInfraSettings } from '../../../settings';
import { newDocumentRequestInit } from './tools';

export async function updateDocumentCommand(
  projectId: number,
  id: number,
  file: File
) {
  const serverUri = getInfraSettings().api.url;
  const options = await newDocumentRequestInit(file, { method: 'PUT' });
  const response = await fetch(
    `${serverUri}/project/${projectId}/document/${id}`,
    options
  );

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to upload document');
  }

  return await response.json();
}
