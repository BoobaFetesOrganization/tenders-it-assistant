import { gql } from '@apollo/client';

export const GetUserStoryQuery = gql`
  query GetUserStory($projectId: Int!, $groupId: Int!, $id: Int!) {
    userstory(projectId: $projectId, groupId: $groupId, id: $id)
      @rest(
        type: "IUserStoryDto"
        method: "GET"
        path: "/project/{args.projectId}/group/{args.projectId}}/userstory/{args.id}"
      ) {
      id
      name
      cost
      tasks
    }
  }
`;
