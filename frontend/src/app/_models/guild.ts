import {AutoModPlugin} from './auto-mod-plugin';
import {CustomCommandsPlugin} from './custom-commands-plugin';
import {TemporaryBan} from './temporary-ban';
import {User} from './user';
import {ClashAPIPlugin} from './clashapi-plugin';

export interface Guild {
  id: string;
  guildId: string;
  managers: User[];
  clashAPIPlugin: ClashAPIPlugin;
  autoModPlugin: AutoModPlugin;
  customCommandsPlugin: CustomCommandsPlugin;
  temporaryBans: TemporaryBan[];
}
