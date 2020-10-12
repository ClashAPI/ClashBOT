import {User} from './user';

export interface PatchNote {
  id: string;
  title: string;
  content: string;
  commitId: string;
  author: User;
  createdAt: Date;
}
