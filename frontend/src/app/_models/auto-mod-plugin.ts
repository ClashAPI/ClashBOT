import {ModeratorRole} from './moderator-role';
import {Plugin} from './plugin';
import {BadWordsSettings} from './bad-words-settings';
import {RepeatedTextSettings} from './repeated-text-settings';
import {ServerInvitesSettings} from './server-invites-settings';
import {ExternalLinksSettings} from './external-links-settings';
import {ExcessiveCapsSettings} from './excessive-caps-settings';
import {ExcessiveEmojisSettings} from './excessive-emojis-settings';
import {ExcessiveSpoilersSettings} from './excessive-spoilers-settings';
import {ExcessiveMentionsSettings} from './excessive-mentions-settings';
import {ZalgoSettings} from './zalgo-settings';
import {AutomatedAction} from './automated-action';
import {ModeratorCommand} from './moderator-command';

export interface AutoModPlugin extends Plugin {
  badWordsSettings: BadWordsSettings;
  repeatedTextSettings: RepeatedTextSettings;
  serverInvitesSettings: ServerInvitesSettings;
  externalLinksSettings: ExternalLinksSettings;
  excessiveCapsSettings: ExcessiveCapsSettings;
  excessiveEmojisSettings: ExcessiveEmojisSettings;
  excessiveSpoilersSettings: ExcessiveSpoilersSettings;
  excessiveMentionsSettings: ExcessiveMentionsSettings;
  zalgoSettings: ZalgoSettings;
  automatedActions: AutomatedAction[];
  moderatorCommands: ModeratorCommand[];
  moderatorRoles: ModeratorRole[];
  ignoreBots: boolean;
}
