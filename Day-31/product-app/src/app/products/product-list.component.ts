
import {
  Component,
  OnInit,
  HostListener,
  signal,
  computed,
  effect
} from '@angular/core';
import { ProductService } from './product.service';
import { debounceTime, switchMap, distinctUntilChanged } from 'rxjs/operators';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
})
export class ProductListComponent implements OnInit {
  search = new FormControl('');
  loading = false;
  products: any[] = [];
  skip = 0;
  limit = 15;
  total = 0;

 
  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.search.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((query) => {
          this.skip = 0;
          this.products = [];
          this.loading = true;
          return this.productService.searchProducts(query ?? '', this.limit, this.skip);
        })
      )
      .subscribe(res => {
        this.products = res.products;
        this.total = res.total;
        this.loading = false;
      });

    this.search.setValue('');
  }

  @HostListener('window:scroll', [])
  onScroll() {
    if (
      window.innerHeight + window.scrollY >= document.body.offsetHeight - 100 &&
      this.products.length < this.total &&
      !this.loading
    ) {
      this.loadMore();
    }
  }

  loadMore() {
    this.skip += this.limit;
    this.loading = true;
    this.productService
      .searchProducts(this.search.value ?? '', this.limit, this.skip)
      .subscribe((res) => {
        this.products = [...this.products, ...res.products];
        this.loading = false;
      });
  }
}
