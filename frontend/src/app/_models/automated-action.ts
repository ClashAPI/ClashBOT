import {ModerationAction} from './moderation-action';

export interface AutomatedAction {
  id: string;
  moderationAction: ModerationAction;
  infractionLimit: number;
  timeLimitInSeconds: number;
}
