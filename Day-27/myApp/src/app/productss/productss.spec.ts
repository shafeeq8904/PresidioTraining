import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Productss } from './productss';

describe('Productss', () => {
  let component: Productss;
  let fixture: ComponentFixture<Productss>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Productss]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Productss);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
