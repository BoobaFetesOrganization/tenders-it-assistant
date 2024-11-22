import { useProjects } from '@aogenai/infra';
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';
import { memo } from 'react';

export const Collection = memo(() => {
  const { data } = useProjects();

  return (
    <div>
      <h1>Projects</h1>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              {/* Ajoutez d'autres en-têtes de colonnes si nécessaire */}
            </TableRow>
          </TableHead>
          <TableBody>
            {data?.projects.map((project) => (
              <TableRow key={project.id}>
                <TableCell>{project.id}</TableCell>
                <TableCell>{project.name}</TableCell>
                {/* Ajoutez d'autres cellules de colonnes si nécessaire */}
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
});
