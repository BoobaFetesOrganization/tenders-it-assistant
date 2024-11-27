import { IProjectDto } from '@aogenai/domain';
import { Box, Button } from '@mui/material';
import { FC, memo, useCallback, useEffect, useState } from 'react';
import { CreateContent } from './CreateContent';
import { EditContent } from './EditContent';

interface dataProps {
  className?: string;
  data: IProjectDto;
  save: (data: IProjectDto) => void;
  reset: () => IProjectDto;
  remove?: (item: IProjectDto) => void;
}

export const ProjectItem: FC<dataProps> = memo(
  ({ className, data, save, reset, remove }) => {
    const isCreation = !data.id;
    const [project, setProject] = useState<IProjectDto>(data);

    useEffect(() => {
      setProject(data);
    }, [data]);

    const onSave = useCallback(() => {
      save(project);
    }, [project, save]);

    const onReset = useCallback(() => {
      setProject(reset());
    }, [reset]);

    const onDelete = useCallback(() => {
      remove?.(data);
    }, [data, remove]);

    return (
      <Box
        className={className}
        sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
      >
        {isCreation && <CreateContent data={project} setData={setProject} />}
        {!isCreation && <EditContent data={project} setData={setProject} />}
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'end' }}>
          {!isCreation && remove && (
            <Button variant="contained" color="error" onClick={onDelete}>
              Delete
            </Button>
          )}
          <Button variant="outlined" color="secondary" onClick={onReset}>
            Reset
          </Button>
          <Button variant="contained" color="primary" onClick={onSave}>
            Save
          </Button>
        </Box>
      </Box>
    );
  }
);
