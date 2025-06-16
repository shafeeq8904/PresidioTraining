import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private http = inject(HttpClient);

  getProductSearchResult(query: string, limit: number = 10, skip: number = 0): Observable<any> {
    return this.http.get(`https://dummyjson.com/products/search?q=${query}&limit=${limit}&skip=${skip}`);
  }
}