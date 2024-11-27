import { gql } from '@apollo/client';

export const DeleteDocumentMutation = gql`
  mutation DeleteDocument($projectId: Int!, $id: Int!) {
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
