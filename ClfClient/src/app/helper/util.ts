import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { Clf } from '../model/clf.model';

@Injectable({
  providedIn: 'root'
})
export class UtilService {
  setTimeAndZone(requestDate: string, time: string, zone: string): moment.Moment {
    // Concat date string, time string and zone(removing ':' from zone)
    let str = requestDate.substr(0, 11) + time + zone;
    let requestTime = moment(str).utcOffset(str);

    return requestTime;
  }

  getClfFromFormGroup(id: string, clfForm: FormGroup): Clf {
    let requestTime = this.setTimeAndZone(clfForm.controls.requestTime.value, 
                                      clfForm.controls.time.value,
                                      clfForm.controls.timeZone.value);

    return {
      id: id,
      client: clfForm.controls.client.value,
      rfcIdentity: clfForm.controls.rfcIdentity.value,
      userId: clfForm.controls.userId.value,
      requestDate: requestTime,
      requestTime: requestTime,
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
      requestDate: clf.requestDate,
      requestTime: clf.requestTime.creationData().input.toString(),
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
