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

  public getClfByClient(client: string): Observable<Clf> {
    let url = `clf/by-client/${client}`;
    return this.httpClient.get<Clf>(env.apiEndpoint + url);
  }
}
