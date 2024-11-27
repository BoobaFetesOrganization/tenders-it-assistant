import { FC, HTMLAttributes, memo } from 'react';
import { useDropzone } from 'react-dropzone';

interface IDropZoneProps {
  onDrop(acceptedFiles: File[]): void;
  rootProps?: HTMLAttributes<HTMLElement>;
  zoneProps?: HTMLAttributes<HTMLElement>;
}
export const DropZone: FC<IDropZoneProps> = memo(
  ({ onDrop, rootProps, zoneProps }) => {
    const { getRootProps, getInputProps } = useDropzone({
      onDrop,
      accept: {
        'application/pdf': ['.pdf'],
        'image/png': ['.png'],
      },
    });

    return (
      <div
        {...getRootProps(rootProps)}
        style={{
          border: '2px dashed #ccc',
          padding: '0 8px',
          textAlign: 'center',
        }}
      >
        <input {...getInputProps()} />
        <p {...zoneProps} style={{ width: 300, ...zoneProps?.style }}>
          Drop files here, to send them ...
        </p>
      </div>
    );
  }
);
