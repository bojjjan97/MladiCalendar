import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserUpdateComponentComponent } from './user-update-component.component';

describe('UserUpdateComponentComponent', () => {
  let component: UserUpdateComponentComponent;
  let fixture: ComponentFixture<UserUpdateComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserUpdateComponentComponent]
    });
    fixture = TestBed.createComponent(UserUpdateComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
