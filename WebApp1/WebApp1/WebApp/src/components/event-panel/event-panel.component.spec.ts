import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventPanelComponent } from './event-panel.component';

describe('EventPanelComponent', () => {
  let component: EventPanelComponent;
  let fixture: ComponentFixture<EventPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EventPanelComponent]
    });
    fixture = TestBed.createComponent(EventPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
