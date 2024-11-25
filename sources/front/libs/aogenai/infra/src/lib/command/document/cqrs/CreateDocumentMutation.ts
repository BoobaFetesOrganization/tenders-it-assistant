import { gql } from '@apollo/client';

export const CreateDocumentMutation = gql`
  mutation CreateDocument($projectId: Int!) {
    document(projectId: $projectId)
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
