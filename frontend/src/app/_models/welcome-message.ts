import {DiscordChannel} from './discord-channel';

export interface WelcomeMessage {
  id: string;
  isEnabled: boolean;
  discordChannel: DiscordChannel;
  message: string;
}
