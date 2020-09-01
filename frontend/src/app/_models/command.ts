export interface Command {
  id: string;
  commandCall: string;
  description: string;
  response: string;
  prefix: string;
  isEnabled: boolean;
}
