import { Component } from '@angular/core';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [],
  templateUrl: './customer.html',
  styleUrls: ['./customer.css']
})
export class Customer {
  likeCount = 0;
  dislikeCount = 0;

  incrementLike() {
    this.likeCount++;
  }

  incrementDislike() {
    this.dislikeCount++;
  }
}
