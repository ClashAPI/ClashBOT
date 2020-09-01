export interface User {
  userId: string;
  userRoles: any[];
  discriminator: string;
  createdAt: Date;
}
