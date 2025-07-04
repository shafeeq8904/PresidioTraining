import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { UpiPaymentComponent } from "./upi-payment/upi-payment";

@Component({
  selector: 'app-root',
  imports: [UpiPaymentComponent ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'razorpay-upi-demo';
}
