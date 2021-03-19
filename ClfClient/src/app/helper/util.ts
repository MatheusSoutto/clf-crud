import * as moment from 'moment';

export const Util = {
  getDateISOString(mom: moment.Moment, time: string, timeZone: string): string {
    console.log(mom);
    console.log(time);
    console.log(timeZone);
    let date = mom.toDate();
    date.setHours(Number(time.substr(0, 2)));
    date.setMinutes(Number(time.substr(3, 2)));
    date.setSeconds(Number(time.substr(6, 2)));

    // mom.set({
    //   hour: Number(time.substr(0, 2)),
    //   minute: Number(time.substr(3, 2)),
    //   second: Number(time.substr(6, 2))
    // });
    // date.minutes(Number(time.substr(3, 2)));
    // date.seconds(Number(time.substr(6, 2)));
    //date.utcOffset(timeZone.replace(':', ''));
    

    return date.toISOString().slice(0, -5) + timeZone;
  }
};