import { IProjectDto } from '@aogenai/domain';
import SendIcon from '@mui/icons-material/Send';
import {
  Box,
  Button,
  Grid2,
  Paper,
  styled,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';
import { FC, memo } from 'react';

interface IProjectsToEstimateProps {
  data: IProjectDto[];
  onSelect?(id: number): void;
  onEstimate?(item: IProjectDto): void;
}
export const ProjectsToEstimate: FC<IProjectsToEstimateProps> = memo(
  ({ data, onSelect, onEstimate }) => {
    return (
      <StyledContent>
        <TableContainer
          component={Paper}
          sx={{ overflow: 'auto', height: '100%' }}
        >
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>
                  <Typography fontWeight="bold">ID</Typography>
                </TableCell>
                <TableCell sx={{ width: '100%' }}>
                  <Typography fontWeight="bold">Name</Typography>
                </TableCell>
                {onEstimate && (
                  <TableCell>
                    <Typography fontWeight="bold">Actions</Typography>
                  </TableCell>
                )}
              </TableRow>
            </TableHead>
            <TableBody>
              {data.map((project) => (
                <TableRow key={project.id}>
                  <StyledTableCell>{project.id}</StyledTableCell>
                  <StyledTableCell
                    sx={{
                      width: '100%',
                      '&:hover': {
                        textDecoration: onSelect && 'underline',
                      },
                    }}
                    onClick={() => onSelect?.(project.id)}
                  >
                    <Box display="flex" flexDirection="row" gap={1}>
                      <Typography fontWeight="bold">{project.name}</Typography>
                      {!project.documents.length && (
                        <Typography color="error">
                          (at least one document is required)
                        </Typography>
                      )}
                    </Box>
                  </StyledTableCell>
                  {onEstimate && (
                    <StyledTableCell>
                      <Box display="flex" justifyContent="flex-end">
                        <StyledItemButton
                          id={`project-estimate-${project.id}`}
                          onClick={() => onEstimate(project)}
                          endIcon={<SendIcon />}
                          disabled={!project.documents.length}
                        />
                      </Box>
                    </StyledTableCell>
                  )}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </StyledContent>
    );
  }
);

const StyledContent = styled(Grid2)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  flexGrow: 1,
  overflow: 'hidden',
  '& table > tbody> tr > td': { cursor: 'context-menu' },
}));

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  padding: theme.spacing(1),
}));

const StyledItemButton = styled(Button)(({ theme }) => ({
  padding: theme.spacing(0, 2),
}));
