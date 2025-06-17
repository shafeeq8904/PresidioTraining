import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductModel } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private base = 'https://dummyjson.com/products';

  constructor(private http: HttpClient) {}

  searchProducts(query: string, limit: number, skip: number) {
    return this.http.get<any>(`${this.base}/search?q=${query}&limit=${limit}&skip=${skip}`);
  }

  getProductById(id: number) {
    return this.http.get<ProductModel>(`${this.base}/${id}`);
  }

 

}