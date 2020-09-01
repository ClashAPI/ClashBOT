import DateTimeFormat = Intl.DateTimeFormat;

export interface TemporaryBan {
  id: string;
  memberId: bigint;
  username: string;
  discriminator: string;
  expiresAt: Date;
}
