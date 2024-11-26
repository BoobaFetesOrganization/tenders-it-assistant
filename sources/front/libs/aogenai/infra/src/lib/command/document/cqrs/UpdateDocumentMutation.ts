import { gql } from '@apollo/client';

export const UpdateDocumentMutation = gql`
  mutation UpdateDocument($projectId: Int!, $input: IProjectDto!) {
    document(projectId: $projectId, input: $input)
      @rest(
        type: "IDocumentDto"
        method: "PUT"
        path: "project/{args.projectId}/document"
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
