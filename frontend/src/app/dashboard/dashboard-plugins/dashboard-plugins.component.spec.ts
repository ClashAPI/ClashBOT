import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsComponent } from './dashboard-plugins.component';

describe('DashboardPluginsComponent', () => {
  let component: DashboardPluginsComponent;
  let fixture: ComponentFixture<DashboardPluginsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
