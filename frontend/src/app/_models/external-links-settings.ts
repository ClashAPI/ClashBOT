import {PluginSettings} from './plugin-settings';
import {Website} from './website';

export interface ExternalLinksSettings extends PluginSettings {
  allowedWebsites: Website[];
}
