import { scheduleJob } from 'node-schedule';
import { User } from '../../types/user';
import { refreshToken } from './refreshToken';

export function scheduleRefreshToken(user: User) {
  const now = new Date();
  if (user.accessTokenExpireTime && user.accessTokenExpireTime >= now) {
    const refreshTime = new Date(+user.accessTokenExpireTime - 10 * 1000);
    scheduleJob(refreshTime, refreshToken);
  }
}
