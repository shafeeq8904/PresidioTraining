import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { UpiPaymentComponent } from './upi-payment';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('UpiPaymentComponent', () => {
  let component: UpiPaymentComponent;
  let fixture: ComponentFixture<UpiPaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UpiPaymentComponent,
        HttpClientTestingModule,
        ReactiveFormsModule,
        FormsModule,
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UpiPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the form with 4 controls', () => {
    expect(component.paymentForm.contains('name')).toBeTruthy();
    expect(component.paymentForm.contains('email')).toBeTruthy();
    expect(component.paymentForm.contains('contact')).toBeTruthy();
    expect(component.paymentForm.contains('amount')).toBeTruthy();
  });

  it('should make all fields required', () => {
    const form = component.paymentForm;
    form.setValue({ name: '', email: '', contact: '', amount: null });

    expect(form.invalid).toBeTrue();
    expect(form.get('name')?.hasError('required')).toBeTrue();
    expect(form.get('email')?.hasError('required')).toBeTrue();
    expect(form.get('contact')?.hasError('required')).toBeTrue();
    expect(form.get('amount')?.hasError('required')).toBeTrue();
  });

  it('should validate email format', () => {
    const email = component.paymentForm.get('email');
    email?.setValue('invalid-email');
    expect(email?.invalid).toBeTrue();
    expect(email?.hasError('email')).toBeTrue();
  });

  it('should validate contact as 10 digits', () => {
    const contact = component.paymentForm.get('contact');
    contact?.setValue('12345');
    expect(contact?.invalid).toBeTrue();
    expect(contact?.hasError('pattern')).toBeTrue();

    contact?.setValue('1234567890');
    expect(contact?.valid).toBeTrue();
  });

  it('should disable submit button when form is invalid', () => {
    component.paymentForm.setValue({
      name: '',
      email: '',
      contact: '',
      amount: null
    });
    fixture.detectChanges();

    const button = fixture.debugElement.query(By.css('button')).nativeElement;
    expect(button.disabled).toBeTrue();
  });

  it('should call Razorpay with valid form (mocked)', fakeAsync(() => {
  const razorpaySpy = jasmine.createSpy('open');

  // Proper constructor mock
  (window as any).Razorpay = function (this: any, options: any) {
    this.open = razorpaySpy;
  };

  component.paymentForm.setValue({
    name: 'John Doe',
    email: 'john@example.com',
    contact: '9876543210',
    amount: 100
  });

  spyOn(component['http'], 'post').and.returnValue({
    subscribe: (cb: any) => {
      cb({ id: 'order_test', amount: 10000, currency: 'INR' });
    }
  } as any);

  component.payNow();
  tick();

  expect(razorpaySpy).toHaveBeenCalled();
}));
});
