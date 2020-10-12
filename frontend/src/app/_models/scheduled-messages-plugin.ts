import {Plugin} from './plugin';
import {ScheduledMessage} from './scheduled-message';

export interface ScheduledMessagesPlugin extends Plugin {
  scheduledMessages: ScheduledMessage[];
}
