import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerNavbarComponent } from './manager-navbar';

describe('ManagerNavbar', () => {
  let component: ManagerNavbarComponent;
  let fixture: ComponentFixture<ManagerNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerNavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
