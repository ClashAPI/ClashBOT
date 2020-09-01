import {CommandActionType} from './command-action-type';

export interface CommandAction {
  id: string;
  type: CommandActionType;
  roleId: bigint;
  message: string;
}
