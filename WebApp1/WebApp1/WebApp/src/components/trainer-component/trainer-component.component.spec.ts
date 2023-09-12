import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainerComponentComponent } from './trainer-component.component';

describe('TrainerComponentComponent', () => {
  let component: TrainerComponentComponent;
  let fixture: ComponentFixture<TrainerComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TrainerComponentComponent]
    });
    fixture = TestBed.createComponent(TrainerComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
