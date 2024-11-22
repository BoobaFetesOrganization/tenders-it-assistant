import { gql } from '@apollo/client';

export const CreateDocumentMutation = gql`
  mutation CreateDocument {
    document
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
