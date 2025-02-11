import { IInfraSettings, setInfraSettings } from '@aogenai/infra';

// eslint-disable-next-line @typescript-eslint/no-empty-object-type, @typescript-eslint/no-empty-interface
export interface IAppSettings {}

let settings: IAppSettings | undefined = undefined;

export const setAppSettings = (appSettings: IAppSettings) => {
  if (!appSettings) throw new Error('App settings not set');
  settings = appSettings;
};

export const getAppSettings = () => {
  if (!settings) throw new Error('App settings not set');
  return Object.freeze(settings);
};

interface ISettings {
  app: IAppSettings;
  libs: {
    infra: IInfraSettings;
  };
}

export async function loadSettings() {
  try {
    const response = await fetch('assets/appsettings.json');
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data: ISettings = await response.json();

    setAppSettings(data.app);
    setInfraSettings(data.libs['infra']);
  } catch (error) {
    console.error('Failed to load app settings:', error);
    throw error;
  }
}
