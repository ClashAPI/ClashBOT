import {Guild} from './guild';
import {DiscordChannel} from './discord-channel';
import {DiscordRole} from './discord-role';
import {ModerationAction} from './moderation-action';

export interface PluginSettings {
  id: string;
  ignoredChannels: DiscordChannel[];
  allowedRoles: DiscordRole[];
  moderationAction: ModerationAction;
  guild: Guild;
}
