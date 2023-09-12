import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendEmailsFormComponent } from './send-emails-form.component';

describe('SendEmailsFormComponent', () => {
  let component: SendEmailsFormComponent;
  let fixture: ComponentFixture<SendEmailsFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SendEmailsFormComponent]
    });
    fixture = TestBed.createComponent(SendEmailsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
