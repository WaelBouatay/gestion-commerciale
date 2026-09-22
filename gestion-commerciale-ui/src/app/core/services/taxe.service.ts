import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Taxe, TaxePayload } from '../models/taxe.model';

@Injectable({
  providedIn: 'root',
})
export class TaxeService {
  private apiUrl = `${environment.apiUrl}/taxes`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Taxe[]> {
    return this.http.get<Taxe[]>(this.apiUrl);
  }

  create(payload: TaxePayload): Observable<Taxe> {
    return this.http.post<Taxe>(this.apiUrl, payload);
  }

  update(id: number, payload: TaxePayload): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
