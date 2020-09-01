import {Command} from './command';
import {AdvancedCommand} from './advanced-command';
import {Plugin} from './plugin';

export interface CustomCommandsPlugin extends Plugin {
  commands: Command[];
  advancedCommands: AdvancedCommand[];
}
