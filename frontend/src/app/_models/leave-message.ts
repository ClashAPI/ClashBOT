import {DiscordChannel} from './discord-channel';

export interface LeaveMessage {
  id: string;
  isEnabled: boolean;
  discordChannel: DiscordChannel;
  message: string;
}
