import { IInfraSettings } from '@aogenai/infra';
import { appSettings } from './AppSettings';
import { IClientSettings } from './settings';

type SettingsReturnType<T extends 'app' | 'infra'> = T extends 'app'
  ? IClientSettings
  : T extends 'infra'
  ? IInfraSettings
  : never;

export function getSettings<T extends 'app' | 'infra'>(
  name: T
): SettingsReturnType<T> {
  switch (name) {
    case 'app':
      return appSettings.app as SettingsReturnType<T>;
    case 'infra':
      return appSettings.libs['@aogenai/infra'] as SettingsReturnType<T>;
    default:
      throw new Error(`Settings ${name} not found`);
  }
}
