export async function newDocumentRequestInit(
  file: File,
  options: Required<Pick<RequestInit, 'method'>> &
    Omit<Partial<RequestInit>, 'method'>
): Promise<RequestInit> {
  const formData = new FormData();
  formData.append('file', file);

  return {
    ...options,
    method: options.method,
    body: formData,
  };
}
