import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegionDTO } from '../components/region/RegionDTO';
import { environment } from '../../env/env';

@Injectable({
  providedIn: 'root'
})
export class RegionService {
  private apiUrl = `${environment.apiBaseUrl}/Regions`;

  constructor(private http: HttpClient) { }

  // In your region.service.ts
  getAllRegions(page: number = 1, pageSize: number = 9): Observable<{ regions: RegionDTO[], totalCount: number }> {

    return this.http.get<{ regions: RegionDTO[], totalCount: number }>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  createRegion(regionData: any): Observable<any> {
    return this.http.post(this.apiUrl, regionData);
  }

  updateRegion(id: string, regionData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, regionData);
  }

  deleteRegion(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }


}