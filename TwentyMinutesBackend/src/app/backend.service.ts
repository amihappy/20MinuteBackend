import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { pipe, from, forkJoin, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable()
export class BackendService {

  constructor(private http: HttpClient) {
  }

  createNewBackendWithJson(json: string) {
    return this.createBackend(json).pipe(
    map(response => {
      const url = response.body.toString();
      const call = this.http.get(url);
      return forkJoin([call, of(url)]);
    }),
    switchMap(data => {
      return data;
    }));
  }

  private createBackend(json: string){
    const createBackendUrl = environment.apiUrl + '/api/Backend/Create';
    return this.http.post(createBackendUrl, json,
      {
        headers: { 'Content-Type': 'application/json' },
        observe: 'response'
      });
  }
}
