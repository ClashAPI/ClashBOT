import {PluginSettings} from './plugin-settings';
import {BadWord} from './bad-word';

export interface BadWordsSettings extends PluginSettings {
  badWords: BadWord[];
}
