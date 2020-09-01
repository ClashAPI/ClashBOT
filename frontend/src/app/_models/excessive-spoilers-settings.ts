import {PluginSettings} from './plugin-settings';

export interface ExcessiveSpoilersSettings extends PluginSettings {
  spoilerTagsLimit: number;
}
