import {AppPreferences} from './app-preferences';

export interface User {
  userId: string;
  userRoles: any[];
  discriminator: string;
  createdAt: Date;
  preferences: AppPreferences;
}
