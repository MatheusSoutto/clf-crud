import { Moment } from "moment";

export class Clf {
  id: string;
  client: string;
  rfcIdentity: string;
  userId: string;
  requestDate: Moment;
  method: string;
  request: string;
  protocol: string;
  statusCode: number;
  responseSize: number;
  referrer: string;
  userAgent: string;
}