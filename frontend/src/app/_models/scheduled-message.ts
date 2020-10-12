import {Guild} from './guild';

export interface ScheduledMessage {
  id: string;
  message: string;
  interval: number;
  channelId: string;
  guild: Guild;
}
