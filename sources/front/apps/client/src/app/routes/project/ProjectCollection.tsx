import { Project } from '@aogenai/application';
import SendIcon from '@mui/icons-material/Send';
import { Button, styled } from '@mui/material';
import { FC, memo } from 'react';
import { useNavigate } from 'react-router';

export const ProjectCollection: FC = memo(() => {
  const navigate = useNavigate();
  return (
    <Project.Collection
      onCreate={() => navigate({ pathname: '/project/0' })}
      onActions={({ id }) => {
        return (
          <StyledButton
            id={`project-navigate-${id}`}
            onClick={() => navigate({ pathname: `/project/${id}` })}
            endIcon={<SendIcon />}
          />
        );
      }}
    />
  );
});

const StyledButton = styled(Button)(({ theme }) => ({
  padding: theme.spacing(0, 2),
}));
