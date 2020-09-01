import {PluginSettings} from './plugin-settings';

export interface ExcessiveMentionsSettings extends PluginSettings {
  mentionsLimit: number;
}
