import {DiscordRole} from './discord-role';
import {WelcomeMessage} from './welcome-message';
import {PrivateWelcomeMessage} from './private-welcome-message';
import {LeaveMessage} from './leave-message';
import {Plugin} from './plugin';

export interface WelcomePlugin extends Plugin {
  welcomeMessage: WelcomeMessage;
  privateWelcomeMessage: PrivateWelcomeMessage;
  rolesToGive: DiscordRole[];
  leaveMessage: LeaveMessage;
}
