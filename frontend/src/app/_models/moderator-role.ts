import {AutoModPlugin} from './auto-mod-plugin';

export interface ModeratorRole {
  id: string;
  roleId: bigint;
  autoModPlugin: AutoModPlugin;
}
