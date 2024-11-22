import { gql } from '@apollo/client';

export const UpdateDocumentMutation = gql`
  mutation UpdateDocument {
    document @rest(type: "IDocumentDto", method: "PUT", path: "/document") {
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
