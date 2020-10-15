import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsWelcomeComponent } from './dashboard-plugins-welcome.component';

describe('DashboardPluginsWelcomeComponent', () => {
  let component: DashboardPluginsWelcomeComponent;
  let fixture: ComponentFixture<DashboardPluginsWelcomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsWelcomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsWelcomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
