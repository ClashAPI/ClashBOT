import {Plugin} from './plugin';
import {TwitchChannelSubscription} from './twitch-channel-subscription';

export interface TwitchPlugin extends Plugin {
  twitchChannelSubscriptions: TwitchChannelSubscription[];
}
