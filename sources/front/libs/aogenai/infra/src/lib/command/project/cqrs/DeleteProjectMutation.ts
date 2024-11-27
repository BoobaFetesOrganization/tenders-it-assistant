import { gql } from '@apollo/client';

export const DeleteProjectMutation = gql`
  mutation DeleteProject($id: Int!) {
    document(id: $id)
      @rest(type: "IProjectDto", method: "DELETE", path: "/project/{args.id}") {
      id
      name
      prompt
      responseId
      documents
      userStories
    }
  }
`;
