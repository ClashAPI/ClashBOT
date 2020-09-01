import {CommandAction} from './command-action';

export interface AdvancedCommand {
  id: string;
  commandCall: string;
  description: string;
  prefix: string;
  actions: CommandAction[];
  isEnabled: boolean;
}
