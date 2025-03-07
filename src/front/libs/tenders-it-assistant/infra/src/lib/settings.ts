export interface IInfraSettings {
  api: {
    url: string;
    maxLimit: number;
  };
}

let settings: IInfraSettings | undefined = undefined;

export const setInfraSettings = (infraSettings: IInfraSettings) => {
  if (!infraSettings) throw new Error('App settings not set');
  settings = infraSettings;
};

export const getInfraSettings = () => {
  if (!settings) throw new Error('Infra settings not set');
  return Object.freeze(settings);
};
