import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Clf } from '../model/clf.model';
import { environment as env } from "./../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ClfService {

  httpOptions = {
    headers: new HttpHeaders({
      'Content-type': 'application/json'
    })
  };

  constructor(
    private httpClient: HttpClient
  ) { }
  
  public getClfs(): Observable<Clf[]> {
    let url = `clf`;
    console.log(env.apiEndpoint);
    return this.httpClient.get<Clf[]>(env.apiEndpoint + url);
  }

  public getClfsByClient(client: string): Observable<Clf[]> {
    let url = `clf/by-client/${client}`;
    return this.httpClient.get<Clf[]>(env.apiEndpoint + url);
  }

  public getClfsByRequestDate(requestDate: Date): Observable<Clf[]> {
    let url = `clf/by-request-date/${requestDate.toString()}`;
    return this.httpClient.get<Clf[]>(env.apiEndpoint + url);
  }

  public getClfsByUserAgent(userAgent: string): Observable<Clf[]> {
    let url = `clf/by-user-agent?userAgent${userAgent}`;
    return this.httpClient.get<Clf[]>(env.apiEndpoint + url);
  }

  public postClf(clf: Clf) : Observable<any> {
    return this.httpClient.post<Clf>(env.apiEndpoint + 'clf', clf, this.httpOptions);
  }
}
