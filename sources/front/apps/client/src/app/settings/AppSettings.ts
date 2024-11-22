import { IInfraSettings } from '@aogenai/infra';
import { IClientSettings } from './settings';

declare global {
  interface Window {
    appsettings?: IAppSettings;
  }
}

export interface IAppSettings {
  app: IClientSettings;
  libs: {
    '@aogenai/infra': IInfraSettings;
  };
}

let _appsettings: IAppSettings | undefined = undefined;

await (async () => {
  try {
    const response = await fetch('assets/appsettings.json');
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data: IAppSettings = await response.json();
    _appsettings = data;
  } catch (error) {
    console.error('Failed to load app settings:', error);
    throw error;
  }
})();

// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
export const appSettings: IAppSettings = Object.freeze(_appsettings!);
