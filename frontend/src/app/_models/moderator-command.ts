export interface ModeratorCommand {
  id: string;
  commandCall: string;
  description: string;
  response: string;
  prefix: string;
  isEnabled: boolean;
}
