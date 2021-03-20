import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { Clf } from '../model/clf.model';

@Injectable({
  providedIn: 'root'
})
export class UtilService {
  setTimeAndZone(requestDate: moment.Moment, time: string, zone: string): moment.Moment {
    requestDate = moment(requestDate);

    console.log(requestDate);
    requestDate.set({
      hour: Number(time.substr(0, 2)),
      minute: Number(time.substr(3, 2)),
      second: Number(time.substr(6, 2))
    })

    // utcOffset() only sets the UTC flag, not actually change the date
    requestDate = moment(
      requestDate.utcOffset(zone, true).format()
    );
    console.log(requestDate);

    return requestDate;
  }

  getClfFromFormGroup(id: string, clfForm: FormGroup): Clf {
    let requestDate = this.setTimeAndZone(clfForm.controls.requestDate.value, 
                                      clfForm.controls.time.value,
                                      clfForm.controls.timeZone.value);
    // let requestDate: moment.Moment = moment(clfForm.controls.requestDate.value);

    console.log(requestDate);
    requestDate.set({
      hour: Number(clfForm.controls.time.value.substr(0, 2)),
      minute: Number(clfForm.controls.time.value.substr(3, 2)),
      second: Number(clfForm.controls.time.value.substr(6, 2))
    })

    // utcOffset() only sets the UTC flag, not actually change the date
    requestDate = moment(
      requestDate.utcOffset(clfForm.controls.timeZone.value, true).format()
    );
    console.log(requestDate);

    return {
      id: id,
      client: clfForm.controls.client.value,
      rfcIdentity: clfForm.controls.rfcIdentity.value,
      userId: clfForm.controls.userId.value,
      requestDate: requestDate,
      method: clfForm.controls.method.value,
      request: clfForm.controls.request.value,
      protocol: clfForm.controls.protocol.value,
      statusCode: Number(clfForm.controls.statusCode.value),
      responseSize: Number(clfForm.controls.responseSize.value),
      referrer: clfForm.controls.referrer.value,
      userAgent: clfForm.controls.userAgent.value
    };
  }

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
