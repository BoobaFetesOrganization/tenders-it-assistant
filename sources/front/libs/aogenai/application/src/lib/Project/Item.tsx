import { IProjectDto } from '@aogenai/domain';
import { Box, Button, TextField } from '@mui/material';
import { ChangeEvent, FC, memo, useEffect, useState } from 'react';

interface dataProps {
  data: IProjectDto;
  save: (data: IProjectDto) => void;
  reset: () => IProjectDto;
  mode?: 'create';
}

export const ProjectItem: FC<dataProps> = memo(
  ({ data, save, reset, mode }) => {
    const [project, setProject] = useState<IProjectDto>(data);

    useEffect(() => {
      setProject(data);
    }, [data]);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;
      setProject({
        ...project,
        [name]: value,
      });
    };

    const handleSave = () => {
      save(project);
    };

    const handleReset = () => {
      setProject(reset());
    };

    return (
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        <TextField
          label="Name"
          name="Name"
          value={project.prompt}
          onChange={handleChange}
          variant="outlined"
        />
        {mode !== 'create' && (
          <>
            <TextField
              label="Prompt"
              name="prompt"
              value={project.prompt}
              onChange={handleChange}
              variant="outlined"
            />
            <TextField
              label="Response ID"
              name="responseId"
              type="number"
              value={project.responseId}
              onChange={handleChange}
              variant="outlined"
            />
          </>
        )}

        {/* Add more inputs for documents and userStories if needed */}
        <Button variant="contained" color="primary" onClick={handleSave}>
          Save
        </Button>
        <Button variant="outlined" color="secondary" onClick={handleReset}>
          Reset
        </Button>
      </Box>
    );
  }
);
