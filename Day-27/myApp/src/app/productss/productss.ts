import { Component, OnInit } from '@angular/core';
import { ProductService } from '../Services/Product.services';
import { ProductModel } from '../models/model';
import { Products } from "../products/products";


@Component({
  selector: 'app-products',
  imports: [Products],
  templateUrl: './productss.html',
  styleUrl: './productss.css'
})
export class Products implements OnInit {
  products:ProductModel[]|undefined=undefined;
  constructor(private ProductService:ProductService){

  }
  ngOnInit(): void {
    this.ProductService.getAllProducts().subscribe(
      {
        next:(data:any)=>{
         this.products = data.products as ProductModel[];
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
  }

}