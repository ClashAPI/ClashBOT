import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsScheduledMessagesComponent } from './dashboard-plugins-scheduled-messages.component';

describe('DashboardPluginsScheduledMessagesComponent', () => {
  let component: DashboardPluginsScheduledMessagesComponent;
  let fixture: ComponentFixture<DashboardPluginsScheduledMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsScheduledMessagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsScheduledMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
