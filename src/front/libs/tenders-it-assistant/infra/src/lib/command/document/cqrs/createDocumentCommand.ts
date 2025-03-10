import { getInfraSettings } from '../../../settings';
import { newDocumentRequestInit } from './tools';

export async function createDocumentCommand(projectId: string, file: File) {
  const serverUri = getInfraSettings().api.url;
  const options = await newDocumentRequestInit(file, { method: 'POST' });
  const response = await fetch(
    `${serverUri}/project/${projectId}/document`,
    options
  );

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to create document');
  }

  return await response.json();
}
