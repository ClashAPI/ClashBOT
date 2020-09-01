import {Guild} from './guild';

export interface BadWord {
  id: string;
  word: string;
  guild: Guild;
}
