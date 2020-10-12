import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsTwitchComponent } from './dashboard-plugins-twitch.component';

describe('DashboardPluginsTwitchComponent', () => {
  let component: DashboardPluginsTwitchComponent;
  let fixture: ComponentFixture<DashboardPluginsTwitchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsTwitchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsTwitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
