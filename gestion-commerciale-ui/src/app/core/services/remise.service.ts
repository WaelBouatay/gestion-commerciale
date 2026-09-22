import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Remise, RemisePayload } from '../models/remise.model';

@Injectable({
  providedIn: 'root',
})
export class RemiseService {
  private apiUrl = `${environment.apiUrl}/remises`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Remise[]> {
    return this.http.get<Remise[]>(this.apiUrl);
  }

  create(payload: RemisePayload): Observable<Remise> {
    return this.http.post<Remise>(this.apiUrl, payload);
  }

  update(id: number, payload: RemisePayload): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
