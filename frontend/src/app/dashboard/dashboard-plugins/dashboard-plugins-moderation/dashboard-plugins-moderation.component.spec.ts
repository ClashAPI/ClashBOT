import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsModerationComponent } from './dashboard-plugins-moderation.component';

describe('DashboardPluginsModerationComponent', () => {
  let component: DashboardPluginsModerationComponent;
  let fixture: ComponentFixture<DashboardPluginsModerationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsModerationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsModerationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
