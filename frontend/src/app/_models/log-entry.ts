import {ActionType} from './action-type';
import {User} from './user';

export interface LogEntry {
  id: string;
  actionName: string;
  actionType: ActionType;
  initiator: User;
  date: Date;
  guildId?: string;
}
