import { gql } from '@apollo/client';

export const GetUserStoryQuery = gql`
  query GetUserStory($projectId: String!, $groupId: String!, $id: String!) {
    story(projectId: $projectId, groupId: $groupId, id: $id)
      @rest(
        type: "IUserStoryDto"
        method: "GET"
        path: "/project/{args.projectId}/group/{args.groupId}/userstory/{args.id}"
      ) {
      id
      groupId
      name
      cost
      tasks
    }
  }
`;
