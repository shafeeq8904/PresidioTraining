// src/app/home/home.ts
import {
  Component,
  OnInit,
  HostListener,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductModel } from '../models/product.model';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
})
export class Home implements OnInit {
  private productService = inject(ProductService);

  products: ProductModel[] = [];
  searchString: string = '';
  loading: boolean = false;
  limit = 10;
  skip = 0;
  total = 0;
  private debounceTimer: any;

  ngOnInit(): void {
    this.loadProducts();
  }

  handleSearchProducts() {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      this.skip = 0;
      this.products = [];
      this.loadProducts();
    }, 400);
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - 100;
    if (scrollPosition >= threshold && this.products.length < this.total && !this.loading) {
      this.loadMore();
    }
  }

  loadProducts() {
    this.loading = true;
    this.productService
      .getProductSearchResult(this.searchString, this.limit, this.skip)
      .subscribe({
        next: (data: any) => {
          this.products = [...this.products, ...data.products];
          this.total = data.total;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        },
      });
  }

  loadMore() {
    this.skip += this.limit;
    this.loadProducts();
  }

  trackById(index: number, item: ProductModel) {
    return item.id;
  }
}
