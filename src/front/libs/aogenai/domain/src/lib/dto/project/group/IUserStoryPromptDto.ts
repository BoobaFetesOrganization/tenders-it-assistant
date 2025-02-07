export interface IUserStoryPromptDto {
  context: string;
  personas: string;
  tasks: string;
}

export function newUserStoryPromptDto(
  obj?: Partial<IUserStoryPromptDto>
): IUserStoryPromptDto {
  return {
    context: obj?.context || '',
    personas: obj?.personas || '',
    tasks: obj?.tasks || '',
  };
}
