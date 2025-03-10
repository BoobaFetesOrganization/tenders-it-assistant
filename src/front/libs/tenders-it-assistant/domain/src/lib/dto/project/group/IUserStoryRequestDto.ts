export interface IUserStoryRequestDto {
  context: string;
  personas: string;
  tasks: string;
}

export function newUserStoryRequestDto(
  obj?: Partial<IUserStoryRequestDto>
): IUserStoryRequestDto {
  return {
    context: obj?.context || '',
    personas: obj?.personas || '',
    tasks: obj?.tasks || '',
  };
}
