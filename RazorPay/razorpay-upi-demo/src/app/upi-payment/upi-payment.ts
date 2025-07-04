import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

declare var Razorpay: any;

@Component({
  selector: 'app-upi-payment',
  standalone: true,
  templateUrl: './upi-payment.html',
  styleUrls: ['./upi-payment.css'],
  imports : [CommonModule , FormsModule ,ReactiveFormsModule]
})
export class UpiPaymentComponent implements OnInit {
  paymentForm!: FormGroup;
  paymentStatus = '';

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      amount: [null, [Validators.required, Validators.min(1)]]
    });
  }

  get f() {
    return this.paymentForm.controls;
  }

  payNow() {
    if (this.paymentForm.invalid) return;

    const form = this.paymentForm.value;

    this.http.post<any>('http://localhost:5080/api/razorpay/create-order', {
      amount: form.amount
    }).subscribe(order => {
      const options = {
        key: 'rzp_test_c6prLkpX6pGyV3',
        amount: order.amount,
        currency: order.currency,
        name: form.name,
        order_id: order.id,
        prefill: {
          name: form.name,
          email: form.email,
          contact: form.contact
        },
        method: {
          upi: true
        },
        handler: (response: any) => {
          this.paymentStatus = `✅ Payment Successful! ID: ${response.razorpay_payment_id}`;
          alert(`✅ Payment Successful!\nPayment ID: ${response.razorpay_payment_id}`);
        },
        modal: {
          ondismiss: () => {
            this.paymentStatus = '❌ Payment Cancelled';
          }
        }
      };

      const rzp = new Razorpay(options);
      rzp.open();
    });
  }
}




