import { gql } from '@apollo/client';

export const DeleteDocumentMutation = gql`
  mutation DeleteDocument($projectId: String!, $id: String!) {
    document(projectId: $projectId, id: $id)
      @rest(
        type: "IDocumentDto"
        method: "DELETE"
        path: "/project/{args.projectId}/document/{args.id}"
      ) {
      id
      name
      content
      createTime
      updateTime
    }
  }
`;
