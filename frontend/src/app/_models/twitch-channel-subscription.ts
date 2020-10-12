import {Guild} from './guild';

export interface TwitchChannelSubscription {
  id: string;
  streamerId: string;
  channelId: string;
  guild: Guild;
}
