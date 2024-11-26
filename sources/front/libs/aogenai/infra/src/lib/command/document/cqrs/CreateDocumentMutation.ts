import { gql } from '@apollo/client';

export const CreateDocumentMutation = gql`
  mutation CreateDocument($projectId: Int!, $input: IProjectDto!) {
    document(projectId: $projectId, input: $input)
      @rest(
        type: "IDocumentDto"
        method: "POST"
        path: "/project/{args.projectId}/document"
      ) {
      id
      name
      prompt
      responseId
      documents {
        id
        name
      }
      userStories {
        id
        name
        cost
      }
    }
  }
`;
