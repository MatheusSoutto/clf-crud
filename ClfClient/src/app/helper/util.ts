import * as moment from 'moment';
import { Clf } from '../model/clf.model';

export const Util = {
  getDateISOString(mom: moment.Moment, time: string, timeZone: string): string {
    console.log(mom.toString());
    console.log(time);
    console.log(timeZone);
    mom.set({
      hours: Number(time.substr(0, 2)),
      minutes: Number(time.substr(3, 2)),
      seconds: Number(time.substr(6, 2))
    });
    mom.utcOffset(timeZone);

    console.log(mom);
    console.log(mom.format());
    // let date = new Date(mom.toISOString());
    // date.setHours(Number(time.substr(0, 2)));
    // date.setMinutes(Number(time.substr(3, 2)));
    // date.setSeconds(Number(time.substr(6, 2)));
    // console.log(date);

    // mom.set({
    //   hour: Number(time.substr(0, 2)),
    //   minute: Number(time.substr(3, 2)),
    //   second: Number(time.substr(6, 2))
    // });
    // date.minutes(Number(time.substr(3, 2)));
    // date.seconds(Number(time.substr(6, 2)));
    //date.utcOffset(timeZone.replace(':', ''));


    return mom.format();
  },

  getJsonFromClf(clf: Clf): object {
    return {
      id: clf.id,
      client: clf.client,
      rfcIdentity: clf.rfcIdentity,
      userId: clf.userId,
      requestDate: clf.requestDate.creationData().input,
      method: clf.method,
      request: clf.request,
      protocol: clf.protocol,
      statusCode: clf.statusCode,
      responseSize: clf.responseSize,
      referrer: clf.referrer,
      userAgent: clf.userAgent
    }
  }
};