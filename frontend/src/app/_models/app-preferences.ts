import {Language} from './language';
import {Theme} from './theme';

export interface AppPreferences {
  id: string;
  language: Language;
  theme: Theme;
}
