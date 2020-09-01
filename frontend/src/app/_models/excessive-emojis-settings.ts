import {PluginSettings} from './plugin-settings';

export interface ExcessiveEmojisSettings extends PluginSettings {
  emojiLimit: number;
}
